using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class OnlineBattleManager : MonoBehaviour, IBattleManager
    {
        [SerializeField] private Animator _bubbleAnimator;
        [SerializeField] private Text _bubbleText;

        [SerializeField] private PentaPuzzleManager _boardGame;
        [SerializeField] private CanvasGroup _boardCanvas;
        [SerializeField] private Character _player;
        [SerializeField] private Character _enemy;

        [SerializeField] private Text _playerActivatedWords;
        [SerializeField] private Text _enemyActivatedWords;

        [SerializeField] private TurnLabel[] _turnLabels;
        [SerializeField] private GameObject _changeScrollButton;

        private bool _isPlayerTurn = true;
        private bool _enemyDead;

        private string _lastWord;
        private bool _enemyAttackReady = false;
        private bool _enemyChangeScrollReady = false;

        [CanBeNull] private Action<string> _endTurnCallback;
        [CanBeNull] private Action<bool> _endBattleCallback;


        private void OnEnable()
        {
            _boardGame.AddWordActivationCallback(OnWordActivation);
            _boardGame.AddExtraActionCallback(OnSkipScroll);
            _player.SetDeadCallback(OnCharacterDead);
            _player.SetDeadAnimEndCallback(OnDeadAnimationEnd);
            _player.SetEndTurnCallback(OnEndTurn);
            _player.GetDamageDealer().SetTarget(_enemy);
            _enemy.SetDeadCallback(OnCharacterDead);
            _enemy.SetDeadAnimEndCallback(OnDeadAnimationEnd);
            _enemy.SetEndTurnCallback(OnEndTurn);

            foreach (var turnLabel in _turnLabels)
            {
                turnLabel.setBossIcon(_enemy.GetTurnIcon());
            }
        }


        public void FixedUpdate()
        {
            if (_enemyAttackReady)
            {
                _enemyAttackReady = false;
                enemyAction(_lastWord);
            }

            if (_enemyChangeScrollReady)
            {
                _enemyChangeScrollReady = false;
                EnemyChangeScroll();
            }
        }

        public void StartBattle(bool isPlayerTurn, string bossName)
        {
            Debug.Log("StartBattle");
            if (!isPlayerTurn)
                ConnectionManager.GetMsg(SetEnemyActionReady);

            _isPlayerTurn = isPlayerTurn;
            _boardCanvas.blocksRaycasts = _isPlayerTurn;

            foreach (var turnLabel in _turnLabels)
            {
                turnLabel.setTurn(_isPlayerTurn);
            }

            _enemy = gameObject.transform.Find(bossName)?.GetComponent<Character>();
            if (_enemy == null)
            {
                Debug.LogError("Bad boss name");
            }
            _enemyActivatedWords = _enemy.transform
                                         .Find("ActivatedWordsPlaceholder")
                                         .Find("ActivatedWords")
                                         .GetComponent<Text>();

            gameObject.SetActive(true);
            _player.gameObject.SetActive(true);
            _enemy.gameObject.SetActive(true);
            _boardGame.gameObject.SetActive(true);
            _boardGame.StartBoardGame();
        }

        public void SetEndTurnCallback(Action<string> callback)
        {
            _endTurnCallback += callback;
        }

        public void SetBattleEndCallback(Action<bool> callback)
        {
            _endBattleCallback += callback;
        }



        public void SetEnemyActionReady(string word)
        {
            if (word[0] == '-')
            {
                _enemyChangeScrollReady = true;
                return;
            }

            if (word.Length > 1)
            {
                _lastWord = word;
                _enemyAttackReady = true;
            }
        }
        // todo: move to character, AI should depend on boss.
        public void enemyAction(string word)
        {
            if (_enemy.IsDead()) return;
            
            List<string> words = _boardGame.GetSelectableWords();
            StartCoroutine(_boardGame.EmulateWordActivation(word));
        }
        
        public void EnemyChangeScroll()
        {
            Debug.Log("EnemyChangeScroll");
            _boardGame.ExtraAction();
            ConnectionManager.GetMsg(SetEnemyActionReady);
        }


        private void OnWordActivation(string word, SpellEffect effect)
        {
            if (_isPlayerTurn)
            {
                _player.Attack(word.Length);
                _bubbleText.text = word.ToUpper() + "!";
                _bubbleAnimator.SetTrigger("Show");
                _player.Attack(word.Length);
                _playerActivatedWords.text = _playerActivatedWords.text + word + "\n";
                _endTurnCallback(word);
            }
            else
            {
                _enemy.Attack(word.Length);
                _enemyActivatedWords.text = _enemyActivatedWords.text + word + "\n";
                _changeScrollButton.SetActive(true);
            }
        }

        private void OnCharacterDead(string deadName)
        {
            Debug.Log(deadName + " is dead!");
            if (_endBattleCallback != null)
            {
                _boardGame.EndBoardGame();
                _enemyDead = !(deadName.Equals(_player.Name));
            }
            else
            {
                Debug.Log("_endBattleCallback is null!");
            }
        }

        private void OnDeadAnimationEnd()
        {
            _endBattleCallback(_enemyDead);
            gameObject.SetActive(false);
        }


        private void OnEndTurn()
        {
            _isPlayerTurn = !_isPlayerTurn;
            _boardCanvas.blocksRaycasts = _isPlayerTurn;

            foreach (var turnLabel in _turnLabels)
            {
                turnLabel.setTurn(_isPlayerTurn);
            }

            // Enemy deals damage to player
            if (!_isPlayerTurn)
            {
                Debug.Log("Boss turn");
                //enemyAction();
            }
            else
            {
                Debug.Log("Player turn");
            }
        }

        private void OnSkipScroll()
        {
            _playerActivatedWords.text = "";
            _enemyActivatedWords.text = "";
        }
    }
}
