using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChessGame
{
    public enum HourglassState
    {
        Wait,
        MyTurn,
        OppTurn
    }
    public class Hourglass : MonoBehaviour
    {
        [SerializeField]
        private Transform rotateCont;
        private GameObject sandPrefab;
        private void Awake()
        {
            sandPrefab = (GameObject)Resources.Load("Hourglass/SandItemPref", typeof(GameObject));
            //
            sandItems = new GameObject[MaxCount];
        }

        private readonly Vector3 createCenter = new Vector3(41.0f, 27.0f, -4.2f);
        private const float CreateScale = 0.00024f;
        private const float CreateRadius = .8f;
        private const int MaxCount = 1300;
        private GameObject[] sandItems;
        private void CreateSand()
        {
            var pos = new Vector3(createCenter.x ,createCenter.y, transform.position.z);
            pos.x += Random.Range(-CreateRadius, CreateRadius);
            pos.y += Random.Range(-CreateRadius, CreateRadius);
            pos.z += Random.Range(-CreateRadius, CreateRadius);
            //
            var obj = Instantiate(sandPrefab, pos, Quaternion.identity, rotateCont);
            obj.transform.localScale = new Vector3(CreateScale, CreateScale, CreateScale);
            //
            sandItems[createdCount] = obj;
            //
            createdCount++;
        }

        private void DestroyAll()
        {
            createdCount = MaxCount;
            for (var i = 0; i < MaxCount; i++)
            {
                if (sandItems[i] != null)
                {
                    Destroy(sandItems[i]);
                }
            }
        }

        private int createdCount;
        private void Update()
        {
            if (createdCount >= MaxCount) return;
            
            for (var i=0; i<30 && createdCount < MaxCount; i++) CreateSand();
        }

        private Tweener tween;
        private HourglassState hourglassState = HourglassState.Wait;
        public void SetState(HourglassState state)
        {
            if (hourglassState == state) return;
            //
            hourglassState = state;
            var rotateAngle = 0;
            int moveTo;
            switch (hourglassState)
            {
                case HourglassState.Wait:
                    moveTo = -4;
                    break;
                case HourglassState.MyTurn:
                    rotateAngle = 180;
                    moveTo = -4-20;
                    break;
                case HourglassState.OppTurn:
                    rotateAngle = -180;
                    moveTo = -4+20;
                    break;
                default:
                    return;
            }

            transform
                .DOMoveZ(moveTo, 0.6f)
                .SetEase(Ease.OutBack);
            //
            DestroyAll();
            if (tween != null)
            {
                tween.Kill(true);
                createdCount = MaxCount;
            }

            if (rotateAngle != 0)
            {
                tween = rotateCont
                    .DORotate(new Vector3(rotateAngle, 0, 0), 1.5f, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.OutBack);
                tween.OnComplete(OnComplete);
            }

        }

        private void OnComplete()
        {
            tween = null;
            createdCount = 0;            
        }
    }
}
