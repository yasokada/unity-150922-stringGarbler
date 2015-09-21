using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonCS : MonoBehaviour {

	public const string rd_file = "indata.txt";
	public const string wr_file = "outdata.txt";
	
	public Text T_result;

	bool garble_string(string instr, out string outstr) {
		int val;

		outstr = "";
		char code;
		int work;
		for(int idx=0; idx < instr.Length; idx++) {
			code = instr[idx];
			if (code == 0x0a || code == 0x0d) { // CRLF
				outstr = outstr + code.ToString();
				continue;
			}
			val = Random.Range(0, 5);
			if (val == 0) {
				work = code + Random.Range(0,5);
				work = Mathf.Max (0x20, work); // SP and later
				work = Mathf.Min(0x7e, work); // up to ~
				code = (char)work;
			}
			outstr = outstr + code.ToString();
		}
		return true;
	}

	void DoGarble_caller() {
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
		DoGarble_caller ();
	}
}
