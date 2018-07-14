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
    /// 選択肢オブジェクト
    /// </summary>
    public class ChoiceObject : MonoBehaviour {

        [SerializeField] private Button button;
        public Button Button => button;

        [SerializeField] private Image image;
        public Image Image => image;

        [SerializeField] private Text text;

        private ReactiveProperty<bool> selected = new ReactiveProperty<bool>(false);
        public IObservable<bool> OnSelected => selected;
        public ReactiveProperty<bool> Selected => selected;

        private Color DefaultColor = ColorUtil.CreateColor(255, 255, 255);
        private Color SelectedColor = ColorUtil.CreateColor(255, 237, 0);

        public string Answer {
            get { return text.text; }
            set { text.text = value; }
        }

        /// <summary>
        /// 選択状態にする
        /// </summary>
        public void Select() {
            image.color = SelectedColor;
        }

        /// <summary>
        /// 非選択状態にする
        /// </summary>
        public void Unselect() {
            image.color = DefaultColor;
        }
    }
}
