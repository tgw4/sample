using WdServerAgent.Model.Config;
using WdServerAgent.Model.Entity.ViewModel.Item;
using WdServerAgent.Model.Service;
using WdServerAgent.Utility;
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
    public class QuizMainBinder : DefaultBinder {

        [SerializeField]
        private QuestionPanel questionPanel;

        [SerializeField]
        private AnswerPanel answerPanel;

        [SerializeField]
        private ScorePanel scorePanel;

        // 現在出題中の問題
        private QuizEntity CurrentQuiz = null;

        // 回答時間
        [HideInInspector]
        public float ElapsedSeconds = 0.0f;

        // 文字数
        private int sentenceCount = 0;

        /// <summary>
        /// HomeMenuのバインダー
        /// </summary>
        public override void Bind(SceneModel model) {

            // 第○問目
            var quizModel = model.FindModel<QuizModel>();
            quizModel.OnChangeCount.Subscribe(count => {
                this.CurrentQuiz = quizModel.CurrentQuiz;
                SetQuiz(count);

                ElapsedSeconds = 0.0f;
            });

            // 解答送信
            quizModel.OnPushAnswer.Subscribe(answer => {

                // 正誤判定＆スコア計算
                var isCorrect = CurrentQuiz.CorrectAnswer == answer;
                if (isCorrect) {
                    var answerTime = QuizConfig.LimitSeconds - ElapsedSeconds;
                    var score = QuizUtil.GetScore(CurrentQuiz, answerTime, false); // TODO:マニアック判定
                    quizModel.Score += score;
                } else {
                    scorePanel.ChangeScore(quizModel.Score, quizModel.Score);
                }
                GeLogger.Debug($"your answer is {answer}. correctAnswer is {CurrentQuiz.CorrectAnswer}");

                // 正誤表示
                scorePanel.ActivateFalsehoodObject(isCorrect);

                // 画面のリセット
                questionPanel.Reset();
                answerPanel.Reset(CurrentQuiz);

                // 次の問題へ
                sentenceCount = 0;
                quizModel.QuizCount++;

            }).AddTo(this);

            // 解答部分のバインド
            answerPanel.Bind(model);

            // 回答時間
            this.UpdateAsObservable().Subscribe(_ => {
                ElapsedSeconds += Time.deltaTime;
                var value = 1 - Mathf.Clamp01(ElapsedSeconds / QuizConfig.LimitSeconds);
                questionPanel.SetSlider(CurrentQuiz.Format, value);

                if (ElapsedSeconds >= QuizConfig.LimitSeconds) {
                    quizModel.PushAnswer("時間切れ");
                }
            });

            // 問題文の表示
            Observable.Interval(TimeSpan.FromSeconds(QuizConfig.SentenceInterval)).Subscribe(_ => {
                questionPanel.ViewSentence(CurrentQuiz, ref sentenceCount);
            }).AddTo(this);

            // スコア変更
            quizModel.OnChangeScore.Buffer(2, 1).Subscribe(scores => {
                var before = scores[0];
                var after = scores[1];
                scorePanel.ChangeScore(before, after);
            }).AddTo(this);
        }

        /// <summary>
        /// クイズを設定します。
        /// </summary>
        public void SetQuiz(int count) {
            questionPanel.SetQuestion(CurrentQuiz, count);
            answerPanel.StartQuiz(CurrentQuiz);
        }
    }
}
