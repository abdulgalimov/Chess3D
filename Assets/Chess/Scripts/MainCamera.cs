using System;
using System.Collections.Generic;
using ChessGame;
using DG.Tweening;
using UnityEngine;


public sealed class MainCamera : MonoBehaviour
{
    private Camera camera;
    private Transform parent;
    public void Start()
    {
        camera = GetComponent<Camera>();
        Debug.LogFormat("camera: {0} / {1}", camera.transform.position, camera.transform.rotation);
        parent = gameObject.transform.parent;
    }
    public GameObject board;
    private void Update()
    {
        if (_waitMode)
        {
            parent.Rotate(new Vector3(0, 1, 0), 0.3f);
        }
        else
        {
            float wheel = Input.GetAxis("Mouse ScrollWheel");
            if (Math.Abs(wheel) > 0.0001f)
            {
                parent.Rotate(new Vector3(0, 1, 0), wheel*10);
            }            
        }
    }

    public Tweener KnightMove(Knight fromPiece, Vector3 toPoint)
    {
        Vector3 direction = toPoint - fromPiece.transform.position;
        direction.Normalize();
        direction = direction*20;
        Vector3 pos = new Vector3(fromPiece.transform.position.x, 40, fromPiece.transform.position.z) - direction;
        //
        float dir = (float)(Math.Atan2(toPoint.x-fromPiece.transform.position.x, toPoint.z-fromPiece.transform.position.z)*180/Math.PI);
        Vector3 rotateVec = new Vector3(30, dir, 0);
        //
        camera.transform.DORotate(rotateVec, 0.6f).SetEase(Ease.OutQuad);
        return camera.transform.DOMove(pos, 0.4f).SetEase(Ease.OutQuad);
    }

    public void Reset()
    {
        camera.transform.DOMove(new Vector3(-4, 74, -65), 0.3f).SetEase(Ease.OutQuad);
        camera.transform.DORotate(new Vector3(49, 0, 0), 0.6f).SetEase(Ease.OutQuad);
    }


    private bool _waitMode;
    public bool WaitMode
    {
        set
        {
            _waitMode = value;
            if (!_waitMode)
            {
                parent.DORotate(new Vector3(0, 0, 0), 0.5f);
            }
        }
    }

}
