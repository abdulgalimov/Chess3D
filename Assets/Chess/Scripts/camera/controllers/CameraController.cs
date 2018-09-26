using UnityEngine;

namespace ChessGame.camera
{
    public class CameraControllerEvents
    {
        public static readonly string ON_COMPLETE = "onComplete";
        public static readonly string ON_EXIT = "onExit";
    }
    public class CameraController : EventEmitter
    {
        static internal Transform cameraParent;
        static internal Camera camera;
        public static void Init(Camera camera)
        {
            cameraParent = camera.gameObject.transform.parent;
            CameraController.camera = camera;
        }

        public void Exit()
        {
            emit(CameraControllerEvents.ON_EXIT);
        }
        
        public virtual void Start()
        {
                        
        }

        public virtual void Stop()
        {
            
        }

        public virtual void Update()
        {
            cameraParent.Rotate(new Vector3(0, 1, 0), 0.3f);            
        }
    }
}
