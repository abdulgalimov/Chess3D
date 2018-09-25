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
        static internal GameObject cameraObject;
        static internal Transform cameraParent;
        static internal Camera camera;
        public static void Init(GameObject cameraObject)
        {
            CameraController.cameraObject = cameraObject;
            cameraParent = cameraObject.transform.parent;
            camera = cameraObject.GetComponent<Camera>();
        }
        public CameraController()
        {
            
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
