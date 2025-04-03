using UnityEngine;

namespace testScript {
    [RequireComponent(typeof(DropComponent))]
    public class TestInputController : MonoBehaviour
    {
        private DropComponent dropComponent;
        public GameObject dropGameObject;
        void Start()
        {
            dropComponent = GetComponent<DropComponent>();
        }


        void Update()
        {
            DropBomb();
        }
        void DropBomb()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                dropComponent.DroppingObject
                    (
                    dropGameObject,
                    new Vector3(transform.position.x, 3, transform.position.z),
                    transform.rotation,
                    null
                    );
            }
        }
    }
}