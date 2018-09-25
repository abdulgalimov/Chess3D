using UnityEngine;

namespace ChessGame.camera
{
    public class CMainController : CameraController
    {
        public CMainController()
        {
            
        }

        public override void Update()
        {
            cameraParent.Rotate(new Vector3(0, 1, 0), 0.3f);
        }
    }
}
