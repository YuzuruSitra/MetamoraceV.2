using System.Network;
using Cinemachine;
using UnityEngine;
using Character;

namespace System.Camera
{
    public class CamManager : MonoBehaviour
    {
        private int _teamID;
        [SerializeField]
        private CinemachineVirtualCameraBase[] _bottomRightCams = new CinemachineVirtualCameraBase[2];
        [SerializeField]
        private CinemachineVirtualCameraBase[] _bottomCenterCams = new CinemachineVirtualCameraBase[2];
        [SerializeField]
        private CinemachineVirtualCameraBase[] _bottomLeftCams = new CinemachineVirtualCameraBase[2];
        [SerializeField]
        private CinemachineVirtualCameraBase[] _topRightCams = new CinemachineVirtualCameraBase[2];
        [SerializeField]
        private CinemachineVirtualCameraBase[] _topCenterCams = new CinemachineVirtualCameraBase[2];
        [SerializeField]
        private CinemachineVirtualCameraBase[] _topLeftCams = new CinemachineVirtualCameraBase[2];
        private GameObject _targetPlayer;
        private PlayerGenerator _playerGenerator;
        
        private void Start()
        {
            _playerGenerator = GameObject.FindWithTag("PlayerGenerator").GetComponent<PlayerGenerator>();
            _targetPlayer = _playerGenerator.CurrentPlayer;
            _teamID = _targetPlayer.GetComponent<CharacterPhotonStatus>().LocalPlayerTeamID - 1;
        }

        private void Update()
        {
            if(_targetPlayer.transform.position.y <= 4.0f)
            {
                switch (_targetPlayer.transform.position.x)
                {
                    case <= -3.5f:
                        ActivateCam(_bottomLeftCams[_teamID]);
                        break;
                    case >= 3.5f:
                        ActivateCam(_bottomRightCams[_teamID]);
                        break;
                    default:
                        ActivateCam(_bottomCenterCams[_teamID]);
                        break;
                }
            }
            else
            {
                switch (_targetPlayer.transform.position.x)
                {
                    case <= -3.5f:
                        ActivateCam(_topLeftCams[_teamID]);
                        break;
                    case >= 3.5f:
                        ActivateCam(_topRightCams[_teamID]);
                        break;
                    default:
                        ActivateCam(_topCenterCams[_teamID]);
                        break;
                }
            }
        }

        private void ActivateCam(CinemachineVirtualCameraBase vcam)
        {
            if(_topLeftCams[_teamID].Priority != 1) _topLeftCams[_teamID].Priority = 1;
            if(_topRightCams[_teamID].Priority != 1) _topRightCams[_teamID].Priority = 1;
            if(_topCenterCams[_teamID].Priority != 1) _topCenterCams[_teamID].Priority = 1;
            if(_bottomLeftCams[_teamID].Priority != 1) _bottomLeftCams[_teamID].Priority = 1;
            if(_bottomRightCams[_teamID].Priority != 1) _bottomRightCams[_teamID].Priority = 1;
            if(_bottomCenterCams[_teamID].Priority != 1) _bottomCenterCams[_teamID].Priority = 1;
            vcam.Priority = 3;
        }

    }
}