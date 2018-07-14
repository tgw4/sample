using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;
using System.Linq;
using WdServerAgent.Model.Entity.ViewModel.Item;
using WdServerAgent.Model.Service;

namespace Wisdom {
    public class QuizModel : MonoBehaviour, ISceneModel {

        // 回答送信
        private Subject<string> pushAnswer = new Subject<string>();
        public IObservable<string> OnPushAnswer => pushAnswer;
        public void PushAnswer(string answer) {
            pushAnswer.OnNext(answer);
        }

        // 出題問題数(第〇問)
        private ReactiveProperty<int> quizCount = new ReactiveProperty<int>(1);
        public IObservable<int> OnChangeCount => quizCount;
        public int QuizCount {
            get { return quizCount.Value; }
            set { quizCount.Value = value; }
        }
        public int Index => QuizCount - 1;

        // 出題クイズ
        private List<QuizEntity> quizList;
        public QuizEntity CurrentQuiz => quizList[Index];

        // 現在の点数
        private ReactiveProperty<int> score = new ReactiveProperty<int>();
        public IObservable<int> OnChangeScore => score;
        public int Score {
            get { return score.Value; }
            set { score.Value = value; }
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize(SceneModel model) {
            quizList = QuizService.Instance.GetAllQuiz();
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Finalize(SceneModel model) {
            quizList = null;
        }
    }
}