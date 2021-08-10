using UnityEngine;

namespace Project.Scripts.Game.Buildings
{
    public class BridgeRender : MonoBehaviour
    {
        [SerializeField] LineRenderer bridgeLine;

        public Vector3 pos1;
        public Vector3 pos2;

        public void SetPoses(Vector3 pos1, Vector3 pos2)
        {
            this.pos1 = pos1;
            this.pos2 = pos2;
        }

        private void Start()
        {
            GameLogic.Instance.bridgeLimoniumPrice += 5;
        }

        private void Update()
        {
            bridgeLine.SetPosition(0, pos1);
            bridgeLine.SetPosition(1, pos2);
        }
    }
}