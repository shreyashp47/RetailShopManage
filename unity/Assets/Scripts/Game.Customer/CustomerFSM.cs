using UnityEngine;
using UnityEngine.AI;

namespace Game.Customer
{
    // CHUNK 3.3: Customer FSM per RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:99
    // Enter → Browse (NavMesh to shelf) → Pick (1-5 basket) → Queue → Pay → Exit
    // Decompiled proof: 52 NavMeshAgents, Agent-Cat/Boy_1 etc. CODE_ANALYSIS:62
    public enum CustomerState { Enter, Browse, Pick, Queue, Pay, Exit }

    [RequireComponent(typeof(NavMeshAgent))]
    public class CustomerFSM : MonoBehaviour
    {
        public CustomerState State { get; private set; } = CustomerState.Enter;
        public float patience = 45f; // 30-60s FR-2.3
        private NavMeshAgent agent;

        void Awake() => agent = GetComponent<NavMeshAgent>();

        void Update()
        {
            patience -= Time.deltaTime;
            if (patience <= 0 && State == CustomerState.Queue)
            {
                // Leave → star drop FR-2.4
                State = CustomerState.Exit;
            }
        }

        public void SetState(CustomerState s) => State = s;

        // TODO: 3.8 wrong-shelf guard: validate shelf.sku == want
    }
}
