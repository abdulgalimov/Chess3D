using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace ChessGame
{
    public enum HourglassState
    {
        WAIT,
        MY_TURN,
        OPP_TURN
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
            _sandsRigidbody = new GameObject[maxCount];
        }

        private readonly Vector3 _createCenter = new Vector3(41.0f, 27.0f, -4.2f);
        private readonly float createScale = 0.00024f;
        private readonly float createRadius = .8f;
        private readonly int maxCount = 1300;
        private GameObject[] _sandsRigidbody;
        private void createSand()
        {
            Vector3 pos = new Vector3(_createCenter.x ,_createCenter.y, transform.position.z);
            pos.x += UnityEngine.Random.Range(-createRadius, createRadius);
            pos.y += UnityEngine.Random.Range(-createRadius, createRadius);
            pos.z += UnityEngine.Random.Range(-createRadius, createRadius);
            //
            GameObject obj = Instantiate(sandPrefab, pos, Quaternion.identity, rotateCont);
            obj.transform.localScale = new Vector3(createScale, createScale, createScale);
            //
            _sandsRigidbody[_createdCount] = obj;
            //
            _createdCount++;
        }

        private void destroyAll()
        {
            _createdCount = maxCount;
            for (int i = 0; i < maxCount; i++)
            {
                if (_sandsRigidbody[i] != null)
                {
                    Destroy(_sandsRigidbody[i]);
                }
            }
        }

        private int _createdCount = 0;
        private void Update()
        {
            if (_createdCount < maxCount)
            {
                for (int i=0; i<30 && _createdCount < maxCount; i++) createSand();
            }
        }

        private Tweener _tween;
        private HourglassState _state = HourglassState.WAIT;
        public void SetState(HourglassState state)
        {
            if (_state == state) return;
            //
            _state = state;
            int rotateAngle = 0;
            int moveTo = 0;
            switch (_state)
            {
                case HourglassState.WAIT:
                    moveTo = -4;
                    break;
                case HourglassState.MY_TURN:
                    rotateAngle = 180;
                    moveTo = -4-20;
                    break;
                case HourglassState.OPP_TURN:
                    rotateAngle = -180;
                    moveTo = -4+20;
                    break;
            }

            transform
                .DOMoveZ(moveTo, 0.6f)
                .SetEase(Ease.OutBack);
            //
            destroyAll();
            if (_tween != null)
            {
                _tween.Kill(true);
                _createdCount = maxCount;
            }

            if (rotateAngle != 0)
            {
                _tween = rotateCont
                    .DORotate(new Vector3(rotateAngle, 0, 0), 1.5f, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.OutBack);
                _tween.OnComplete(onComplete);
            }

        }

        private void onComplete()
        {
            _tween = null;
            _createdCount = 0;            
        }
    }
}
