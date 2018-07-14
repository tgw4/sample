using WdServerAgent.Model.Entity.Define;
using WdServerAgent.Model.Entity.ViewModel.Item;
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
using Wisdom.Data;

namespace Wisdom {
    /// <summary>
    /// 並べ替えクイズ
    /// </summary>
    public class NarabekaeBinder : AnswerBasePanel {
        public override Format format => Format.Narabekae;

        [SerializeField] private List<ChoiceObject> choiceObjects;
        [SerializeField] private Button okButton;

        private int? tempIndex = null;
        private Color NormalColor = ColorUtil.CreateColor(255, 255, 255);
        private Color SelectedColor = ColorUtil.CreateColor(251, 255, 0);

        /// <summary>
        /// バインドします。
        /// </summary>
        public override void Bind(QuizModel model) {

            for (int i = 0; i < choiceObjects.Count; i++) {
                var index = i;
                var button = choiceObjects[index].Button;
                var image = choiceObjects[index].Image;

                button.OnClickAsObservable().Subscribe(_ => {
                    if (tempIndex.HasValue) {

                        // 並べ替え処理
                        var tempText = choiceObjects[index].Answer;
                        choiceObjects[index].Answer = choiceObjects[tempIndex.Value].Answer;
                        choiceObjects[tempIndex.Value].Answer = tempText;

                        // 選択色の解除
                        var selectedImage = choiceObjects[tempIndex.Value].Image;
                        selectedImage.color = NormalColor;

                        tempIndex = null;
                    } else {
                        // 選択状態
                        tempIndex = index;
                        image.color = SelectedColor;
                    }
                }).AddTo(button);
            }

            // OKボタン
            okButton.OnClickAsObservable().Subscribe(_ => {
                var answer = new string(choiceObjects.SelectMany(x => x.Answer).ToArray());
                model.PushAnswer(answer);
            }).AddTo(okButton);
        }

        /// <summary>
        /// 並べ替え問題の初期化を行います。
        /// </summary>
        public override void Initialize(QuizEntity quiz) {
            var shuffleAnswer = quiz.CorrectAnswer.Shuffle();
            for (int i = 0; i < shuffleAnswer.Length; i++) {
                var choiceObject = choiceObjects[i];
                choiceObject.Answer = $"{shuffleAnswer[i]}";
                choiceObject.gameObject.SetActive(true);
            }

            for (int i = shuffleAnswer.Length; i < choiceObjects.Count; i++) {
                var choiceObject = choiceObjects[i];
                choiceObject.Answer = string.Empty;
                choiceObject.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 画面上から消す処理
        /// </summary>
        public override void Reset() {
            for (int i = 0; i < choiceObjects.Count; i++) {
                var choiceObject = choiceObjects[i];
                choiceObject.Answer = string.Empty;
                choiceObject.Image.color = NormalColor;
                choiceObject.gameObject.SetActive(false);
            }
            tempIndex = null;
        }
    }
}
