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
        private Transform _rotateCont;
        private Material _sandMaterial;
        private void Awake()
        {
            _rotateCont = transform.Find("RotateObj");
            _sandMaterial = _rotateCont.Find("Sphere").GetComponent<Renderer>().material;
            //
            _sandsRigidbody = new Rigidbody[1500];
            //
            // Hourglass pos (40,8, 14,3, -4,2);
        }

        private readonly Vector3 _createCenter = new Vector3(41.0f, 27.0f, -4.2f);
        private readonly float createScale = 0.00024f;
        private readonly float createRadius = .8f;
        private readonly int maxCount = 1300;
        private Rigidbody[] _sandsRigidbody;
        private void createSand()
        {
            Vector3 pos = new Vector3(_createCenter.x ,_createCenter.y, transform.position.z);
            pos.x += UnityEngine.Random.Range(-createRadius, createRadius);
            pos.y += UnityEngine.Random.Range(-createRadius, createRadius);
            pos.z += UnityEngine.Random.Range(-createRadius, createRadius);
            //
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.transform.parent = _rotateCont;
            obj.transform.localScale = new Vector3(createScale, createScale, createScale);
            obj.transform.position = pos;
            obj.GetComponent<Renderer>().material = _sandMaterial;
            //
            Rigidbody rigidbody = obj.AddComponent<Rigidbody>();
            rigidbody.freezeRotation = true;
            _sandsRigidbody[_createdCount] = rigidbody;
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
                    Destroy(_sandsRigidbody[i].gameObject);
                }
            }
        }

        private int _createdCount = 1500;
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
                _tween = _rotateCont
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
