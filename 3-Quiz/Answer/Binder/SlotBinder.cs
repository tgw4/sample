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
    /// スロットクイズ
    /// </summary>
    public class SlotBinder : AnswerBasePanel {
        public override Format format => Format.Slot;

        // スロットリール
        [SerializeField] private List<ReelObject> reels;

        // OKボタン
        [SerializeField] private Button okButton;

        /// <summary>
        /// バインドします。
        /// </summary>
        public override void Bind(QuizModel model) {

            for (int i = 0; i < reels.Count; i++) {
                var index = i;
                var reel = reels[index];

                // リールボタン変更時
                reel.OnChangeIndex.Subscribe(reelIndex => {
                    reel.ChangeReel();
                }).AddTo(this);

                // △ボタン
                reel.UpButton.OnClickAsObservable().Subscribe(_ => {
                    reel.AddIndex();
                }).AddTo(reel.UpButton);

                // ▽ボタン
                reel.DownButton.OnClickAsObservable().Subscribe(_ => {
                    reel.SubtractIndex();
                }).AddTo(reel.DownButton);
            }

            // OKボタン
            okButton.OnClickAsObservable().Subscribe(_ => {
                var answer = new string(reels.Where(x => x.isActiveAndEnabled).SelectMany(x => x.Answer).ToArray());
                model.PushAnswer(answer);
            }).AddTo(okButton);
        }

        /// <summary>
        /// 初期化を行います。
        /// </summary>
        public override void Initialize(QuizEntity quiz) {
            Reset();

            var answer = quiz.CorrectAnswer;
            for (int i = 0; i < quiz.CorrectAnswer.Length; i++) {
                var reelStr = new string(quiz.Choices.Select(x => x[i]).OrderBy(_ => Guid.NewGuid()).ToArray());
                var reel = reels[i];
                reel.SetReelStr(reelStr);
            }
        }

        /// <summary>
        /// 画面上から消す処理
        /// </summary>
        public override void Reset() {
            for (int i = 0; i < reels.Count; i++) {
                var reel = reels[i];
                reel.Reset();
            }
        }
    }
}
