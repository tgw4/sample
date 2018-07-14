using WdServerAgent.Model.Entity.Define;
using WdServerAgent.Model.Entity.ViewModel.Item;
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
using Wisdom.Data;

namespace Wisdom {
    /// <summary>
    /// グループ分けクイズ
    /// </summary>
    public class GroupBinder : AnswerBasePanel {
        public override Format format => Format.Group;

        [SerializeField] private List<GameObject> groupParents;
        [SerializeField] private List<Text> groupHeaders;
        [SerializeField] private List<GroupObject> groupObjects;
        [SerializeField] private Button okButton;

        /// <summary>
        /// バインドします。
        /// </summary>
        public override void Bind(QuizModel model) {

            foreach(var groupObject in groupObjects) {

                // 「←」ボタン
                groupObject.LeftButton.OnClickAsObservable().Subscribe(_ => {
                    var groupCount = groupParents.Where(x => x.activeSelf).Count();
                    groupObject.MoveLeft(groupCount);

                    var index = groupObject.GetIndex(groupCount);
                    if (index.HasValue) {
                        groupObject.transform.position = new Vector3(groupParents[index.Value].transform.position.x, groupObject.transform.position.y);
                    }
                }).AddTo(groupObject.LeftButton);

                // 「→」ボタン
                groupObject.RightButton.OnClickAsObservable().Subscribe(_ => {
                    var groupCount = groupParents.Where(x => x.activeSelf).Count();
                    groupObject.MoveRight(groupCount);

                    var index = groupObject.GetIndex(groupCount);
                    if (index.HasValue) {
                        groupObject.transform.position = new Vector3(groupParents[index.Value].transform.position.x, groupObject.transform.position.y);
                    }
                }).AddTo(groupObject.RightButton);
            }

            // OKボタン
            okButton.OnClickAsObservable().Subscribe(_ => {
                var groupCount = groupParents.Where(x => x.activeSelf).Count();
                var answer = new string(groupObjects.FindAll(x => x.isActiveAndEnabled).SelectMany(x => x.GetAnswer(groupCount)).ToArray());
                model.PushAnswer(answer);
            }).AddTo(okButton);
        }

        /// <summary>
        /// 初期化を行います。
        /// </summary>
        public override void Initialize(QuizEntity quiz) {
            Reset();

            // グループの表示
            for(int i = 0; i < quiz.Option; i++) {
                var parent = groupParents[i];
                var header = groupHeaders[i];
                header.text = quiz.Choices[i];
                parent.SetActive(true);
            }

            // グループ分けオブジェクトの表示
            for (int i = quiz.Option; i < quiz.Choices.Count; i++) {
                var groupIndex = i - quiz.Option;
                var groupObject = groupObjects[groupIndex];
                groupObject.Answer = quiz.Choices[i];
                groupObject.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 画面上から消す処理
        /// </summary>
        public override void Reset() {
            foreach(var groupParent in groupParents) {
                groupParent.SetActive(false);
            }

            foreach (var groupObject in groupObjects) {
                groupObject.Answer = string.Empty;
                groupObject.gameObject.SetActive(false);
            }
        }
    }
}
