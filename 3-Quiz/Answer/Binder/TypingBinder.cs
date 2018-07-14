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

namespace Wisdom {
    /// <summary>
    /// タイピングクイズ
    /// </summary>
    public class TypingBinder : AnswerBasePanel {
        public override Format format => Format.Typing;

        // タイピング
        [SerializeField] private List<Text> TypingTexts;

        // キーボード
        [SerializeField] private GameObject keyboardBase;

        // OKボタン
        [SerializeField] private Button okButton;

        // 削除ボタン
        private string DeleteButtonObjectName = "DeleteButton";

        // 解答文字
        private ReactiveProperty<string> _input = new ReactiveProperty<string>(string.Empty);
        private String Input => _input.Value;
        private WordEnum WordEnum;

        /// <summary>
        /// バインドします。
        /// </summary>
        public override void Bind(QuizModel model) {

            // キーボードのバインド
            var keyboardButtons = keyboardBase.GetComponentsInChildren<Button>().ToList();
            var deleteButton = keyboardButtons.Find(x => x.name == DeleteButtonObjectName);

            {
                // 入力ボタン
                foreach (var button in keyboardButtons.Where(x => x.name != DeleteButtonObjectName)) {
                    button.OnClickAsObservable().Subscribe(_ => {
                        var text = button.GetComponentInChildren<Text>();
                        _input.Value = QuizUtil.Convert($"{Input}{text.text}", WordEnum);
                    }).AddTo(button);
                }

                // 削除ボタン
                deleteButton.OnClickAsObservable().Subscribe(_ => {
                    if (!string.IsNullOrEmpty(Input)) {
                        if (Input.Length >= 8) {
                            _input.Value = Input.Substring(0, 7);
                        } else {
                            _input.Value = Input.Substring(0, Input.Length - 1);
                        }
                    }
                }).AddTo(deleteButton);
            }

            // 入力文字列の反映
            _input.Subscribe(input => {
                for(int index = 0; index < TypingTexts.Count && index < input.Length; index++) {
                    TypingTexts[index].text = input[index].ToString();
                }
                for(int index = input.Length; index < TypingTexts.Count; index++) {
                    TypingTexts[index].text = string.Empty;
                }
            }).AddTo(this);

            // OKボタン
            okButton.OnClickAsObservable().Subscribe(_ => {
                var answer = new string(Input.Take(8).ToArray());
                model.PushAnswer(answer);
            }).AddTo(okButton);
        }

        /// <summary>
        /// 初期化を行います。
        /// </summary>
        public override void Initialize(QuizEntity quiz) {
            Reset();

            WordEnum = QuizUtil.GetWordEnum(quiz.CorrectAnswer);
        }

        /// <summary>
        /// 画面上から消す処理
        /// </summary>
        public override void Reset() {
            for (int i = 0; i < TypingTexts.Count; i++) {
                var typingText = TypingTexts[i];
                typingText.text = string.Empty;
            }
            _input.Value = string.Empty;
        }
    }
}
