using UnityEngine;
using UnityEngine.AI;


public class testAgentScript : MonoBehaviour
{

    //removing this line rigth here will NOT cause the agent to dissapear, it still apears
    //assings a target to the agent
    [SerializeField] Transform target;
    
    //agent will dissapear if removed 
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        //agent will dissapear if removed
        agent = GetComponent<NavMeshAgent>();

        //removing this line rigth here, will NOT cause the agent to dissapear, it still apears
        agent.updateRotation = false;
        
        //agent will dissapear if removed
        //assures that the agent only operates on the XY plane
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        //removing this line rigth here will NOT cause the agent to dissapear, it still apears
        agent.SetDestination(target.position);
    }
}
