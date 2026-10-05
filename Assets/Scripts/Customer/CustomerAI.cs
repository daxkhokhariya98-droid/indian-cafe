using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CustomerAI : MonoBehaviour
{
    [Header("Order")]
    [SerializeField] private OrderData[] possibleOrders;

    [Header("UI")]
    [SerializeField] private Transform orderIconAnchor; // customer ke sar ke upar, order icon dikhane ke liye

    public CustomerState State { get; private set; }
    public OrderData CurrentOrder { get; private set; }
    public Transform OrderIconAnchor => orderIconAnchor;

    private NavMeshAgent agent;
    private Transform counterPoint;
    private Transform seatPoint;
    private Transform exitPoint;
    private float patienceTimer;

    public System.Action<CustomerAI> OnOrderExpired;
    public System.Action<CustomerAI> OnCustomerLeft;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Setup(Transform counter, Transform seat, Transform exit)
    {
        counterPoint = counter;
        seatPoint = seat;
        exitPoint = exit;

        State = CustomerState.Entering;
        agent.SetDestination(counterPoint.position);
    }

    private void Update()
    {
        switch (State)
        {
            case CustomerState.Entering:
                HandleArrivalAtCounter();
                break;

            case CustomerState.WaitingForOrder:
                HandlePatienceCountdown();
                break;

            case CustomerState.MovingToSeat:
                HandleArrivalAtSeat();
                break;

            case CustomerState.Eating:
                HandleEatingCountdown();
                break;

            case CustomerState.Leaving:
                HandleArrivalAtExit();
                break;
        }
    }

    private void HandleArrivalAtCounter()
    {
        if (HasArrived())
        {
            State = CustomerState.WaitingAtCounter;
            PlaceOrder();
        }
    }

    private void PlaceOrder()
    {
        CurrentOrder = possibleOrders[Random.Range(0, possibleOrders.Length)];
        patienceTimer = CurrentOrder.patienceTime;
        State = CustomerState.WaitingForOrder;
        // TODO: order icon UI yaha CurrentOrder.icon se update hogi
    }

    private void HandlePatienceCountdown()
    {
        patienceTimer -= Time.deltaTime;
        if (patienceTimer <= 0f)
        {
            OnOrderExpired?.Invoke(this);
            SendToExit();
        }
    }

    // Cooking system isko call karega jab sahi order serve ho
    public void ReceiveOrder(OrderData servedOrder)
    {
        if (State != CustomerState.WaitingForOrder) return;
        if (servedOrder != CurrentOrder) return; // galat order

        if (seatPoint != null)
        {
            State = CustomerState.MovingToSeat;
            agent.SetDestination(seatPoint.position);
        }
        else
        {
            SendToExit();
        }
    }

    private void HandleArrivalAtSeat()
    {
        if (HasArrived())
        {
            State = CustomerState.Eating;
            patienceTimer = 5f; // eating duration
        }
    }

    private void HandleEatingCountdown()
    {
        patienceTimer -= Time.deltaTime;
        if (patienceTimer <= 0f)
            SendToExit();
    }

    private void SendToExit()
    {
        State = CustomerState.Leaving;
        agent.SetDestination(exitPoint.position);
    }

    private void HandleArrivalAtExit()
    {
        if (HasArrived())
            OnCustomerLeft?.Invoke(this);
    }

    private bool HasArrived()
    {
        if (agent.pathPending) return false;
        return agent.remainingDistance <= agent.stoppingDistance + 0.1f;
    }
}
