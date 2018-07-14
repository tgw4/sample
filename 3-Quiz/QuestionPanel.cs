using WdServerAgent.Model.Config;
using WdServerAgent.Model.Entity.Define;
using WdServerAgent.Model.Entity.ViewModel.Item;
using WdServerAgent.Model.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Wisdom {
    /// <summary>
    /// クイズ問題部分
    /// </summary>
    public class QuestionPanel : MonoBehaviour {
        // 第〇問
        [SerializeField]
        private Text QuestionCountText;

        // 正解率
        [SerializeField]
        private Text AccuracyText;

        // 問題文
        [SerializeField]
        private Text SentenceText;

        // 残り回答時間ゲージ
        [SerializeField]
        private Slider Slider;

        [SerializeField]
        private Image SliderImage;

        /// <summary>
        /// 問題文の設定を行います。
        /// </summary>
        public void SetQuestion(QuizEntity quiz, int count) {
            QuestionCountText.text = $"第{count}問";
            AccuracyText.text = $"{quiz.Accuracy}％";
        }

        /// <summary>
        /// 残り回答時間ゲージを設定します。
        /// </summary>
        public void SetSlider(Format format, float value) {
            Slider.value = value;
            SliderImage.color = QuizUtil.GetSliderColor(format, value);
        }

        /// <summary>
        /// 問題文を表示します。
        /// </summary>
        bool isWaiting = false;
        public void ViewSentence(QuizEntity quiz, ref int count) {

            if (isWaiting) return;

            count++;
            if (count > quiz.Sentence.Length) {
                return;
            }

            // 改行が登場したら1秒ウェイト
            int lastIndex = quiz.Sentence.Substring(0, count).LastIndexOf('\n');
            if (lastIndex + 1 == count) {
                isWaiting = true;
                Observable.ReturnUnit().Delay(TimeSpan.FromSeconds(1)).Subscribe(_ => {
                    isWaiting = false;
                }).AddTo(this);
            }
            SentenceText.text = quiz.Sentence.Substring(0, count);
        }

        public void Reset() {
            isWaiting = false;
        }
    }
}
