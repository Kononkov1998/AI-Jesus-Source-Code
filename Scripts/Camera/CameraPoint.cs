using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Camera
{
    public class CameraPoint : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private Follower _camera;

        public void Show()
        {
            _joystick.Stop();
            _joystick.enabled = false;
            _camera.StopFollow();
            DOTween.Sequence()
                .Append(_camera.transform.DOMove(transform.position, 1f))
                .AppendInterval(1f)
                .Append(_camera.transform.DOMove(_camera.TargetPosition, 1f))
                .OnComplete(() =>
                {
                    _camera.StartFollow();
                    _joystick.enabled = true;
                });
        }
    }
}