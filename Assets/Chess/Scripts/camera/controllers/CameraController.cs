using UnityEngine;

namespace ChessGame
{
    public static class CameraControllerEvents
    {
        public const string OnComplete = "onComplete";
        public const string OnExit = "onExit";
    }
    public class CameraController : EventEmitter
    {
        protected static Transform CameraParent;
        protected static Camera CameraObject;
        public static void Init(Camera camera)
        {
            CameraParent = camera.gameObject.transform.parent;
            CameraObject = camera;
        }

        public void Exit()
        {
            Emit(CameraControllerEvents.OnExit);
        }
        
        public virtual void Start()
        {
                        
        }

        public virtual void Stop()
        {
            
        }

        public virtual void Update()
        {
            CameraParent.Rotate(new Vector3(0, 1, 0), 0.3f);            
        }
    }
}
