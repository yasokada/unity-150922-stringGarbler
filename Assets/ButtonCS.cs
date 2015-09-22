using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions; // for Regex


/*
 * v0.2 2015/09/22
 *  - SP is not garbled
 * 
 * v0.1 2015/09/22
 *  - input file is garbled randomly
 */

public class ButtonCS : MonoBehaviour {

	public const string rd_file = "indata.txt";
	public const string wr_file = "outdata.txt";
	
	public Text T_result;
	public InputField IF_percent;

	int get_maxRange(string percentStr) {
		int val;
		val = int.Parse(new Regex("[0-9]+").Match(percentStr).Value);
		if (val == 0) {
			return int.MaxValue; // almost no change
		}
		val = Mathf.Min (100, val); // up to 100
		return (100 / val);
	}

	bool garble_string(string instr, out string outstr) {
		int judge;
		char code;
		int work;

		outstr = "";
		for(int idx=0; idx < instr.Length; idx++) {
			code = instr[idx];
			if (code == 0x0a || code == 0x0d || code == 0x20) { // CRLF or SP
				outstr = outstr + code.ToString();
				continue;
			}
			judge = Random.Range(0, get_maxRange(IF_percent.text));
			if (judge == 0) {
				// randomly change character
				work = code + Random.Range(0,5);

				work = Mathf.Max (0x20, work); // SP and later
				work = Mathf.Min(0x7e, work); // up to ~

				code = (char)work;
			}
			outstr = outstr + code.ToString();
		}
		return true;
	}

	void garble_caller() {
		Debug.Log ("start garble");

		T_result.text = "";

		if (System.IO.File.Exists (rd_file) == false) {
			T_result.text = rd_file + " : not found";
			return;
		}
		string filelines = System.IO.File.ReadAllText (rd_file);

		string reslines = "";
		bool res = garble_string (filelines, out reslines);

		if (res) {
			System.IO.File.WriteAllText (wr_file, reslines);
			T_result.text = "output as " + wr_file;
		}
	}

	public void ButtonClick() {
		garble_caller ();
	}
}
