
public interface ICrossword
{
	/// <summary>
	///
	/// Reads file <b>filepath</b> and creates all the crosswords from it.
	/// <b>Returns</b> the crosswords.
	/// 
	/// </summary>
	//static ICrossword[] LoadCrosswords (string filepath);

	/// <summary>
	///
	/// <b>Returns</b> the appropriate CWWord if there is such word in a crossword,
	///                or null if there isn't.
	/// 
	/// </summary>
	CWword GetCWword (string word); // { return CWDict[word]; }

	/// <summary>
	///
	/// <b>Returns</b> the array of words of the crossword
	/// 
	/// </summary>
	string[] GetWords ();

	/// <summary>
	///
	/// <b>Returns</b> the letters of the crossword to build a Pentagram
	/// 
	/// </summary>
	char[] GetLetters ();

    /// <summary>
    ///
    /// <b>Returns</b> the cheme of a crossword.
    /// 
    /// </summary>
    char[,] GetCrosswordTable ();
}