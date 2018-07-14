using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Wisdom {

    /// <summary>
    /// リールオブジェクト
    /// </summary>
    public class ReelObject : MonoBehaviour {

        [SerializeField] private Button upButton;
        [SerializeField] public Button UpButton => upButton;
        [SerializeField] private Text upText;
        [SerializeField] private Text centerText;
        [SerializeField] private Text downText;
        [SerializeField] private Button downButton;
        [SerializeField] public Button DownButton => downButton;

        private string reelStr = string.Empty;

        // インデックス(一番上のリール)
        private ReactiveProperty<int> index = new ReactiveProperty<int>(0);
        public IObservable<int> OnChangeIndex => index;
        public int Index => index.Value;
        public void AddIndex() {
            index.Value = (index.Value + 1) % 4;
        }
        public void SubtractIndex() {
            if(index.Value == 0) {
                index.Value = 3;
            } else {
                index.Value -= 1;
            }
        }

        /// <summary>
        /// リールの変化時に呼ばれるメソッドです
        /// </summary>
        public void ChangeReel() {
            if (!string.IsNullOrEmpty(reelStr)) {
                upText.text = reelStr[Index].ToString();
                centerText.text = reelStr[(Index + 1) % 4].ToString();
                downText.text = reelStr[(Index + 2) % 4].ToString();
            }
        }

        public string Answer {
            get { return centerText.text; }
        }

        public void SetReelStr(string str) {
            this.reelStr = str;
            ChangeReel();
            this.gameObject.SetActive(true);
        }

        public void Reset() {
            index.Value = 0;
            reelStr = string.Empty;
            upText.text = string.Empty;
            centerText.text = string.Empty;
            downText.text = string.Empty;
            this.gameObject.SetActive(false);
        }
    }
}
