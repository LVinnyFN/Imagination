using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class MushroomSon : MonoBehaviour
{

    private Animator animator;
    private NavMeshAgent navmesh;
    private Transform playerPosition;
    [SerializeField] private Transform portalPosition;

    private bool canfollow = false;
    private bool mushdelivered = false;
    private bool mushsaved = false;
    [SerializeField] private GameObject exclamation;
    [SerializeField] private GameObject dialogbox;
    [SerializeField] private QuestMenu myQuestMenu;
    public int myquestposition;

    public string followsentence;
    public string notfollowsentence;

    private float stopWalkRange = 4;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        navmesh = this.GetComponent<NavMeshAgent>();
        playerPosition = GameController.controller.player.transform;
    }
    private void Update()
    {
        float distance = Vector3.Distance(playerPosition.position, transform.position);
        if (canfollow)
        {
            if (distance > stopWalkRange)
            {
                animator.SetBool("isfollowing", true);
                navmesh.SetDestination(playerPosition.position);
                navmesh.isStopped = false;
            }
            else
            {
                animator.SetBool("isfollowing", false);
                navmesh.isStopped = true;
            }
        }
    }

    public void DeliverMushroom()
    {
        if (canfollow)
        {
            mushdelivered = true;
            canfollow = false;
            GameController.controller.globalStats.MushRescued();
            navmesh.SetDestination(portalPosition.position);
        }
    }

    public void EnterPortal()
    {
        Destroy(this.gameObject);
        //StartCoroutine(DestroyMush());
    }

    public void ActivateMushroom()
    {
        if (!mushsaved && !canfollow && GameController.controller.uiController.questlogPanel.GetComponent<QuestLog>().myQuests.Search(myQuestMenu.quests[myquestposition]) != null)
        {
            exclamation.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") )
        {
            GameController.controller.uiController.ShowAction("talk", true);
        }
        if (canfollow)
        {
            GameController.controller.uiController.ShowAction("talk", false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.controller.uiController.ShowAction("talk", false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && other.CompareTag("Player") && mushdelivered == false && !canfollow)
        {
            GameController.controller.uiController.ShowAction("talk", false);
            if (canfollow == false && exclamation.activeSelf)
            {
                mushsaved = true;
                exclamation.SetActive(false);
                canfollow = true;
                animator.SetBool("isfollowing", true);
                StartCoroutine(Talk(followsentence));

            }
            else
            {
                StartCoroutine(Talk(notfollowsentence));
            }
        }
    }

    private IEnumerator Talk(string sentence)
    {
        GetComponent<BoxCollider>().enabled = false;
        dialogbox.SetActive(true);
        dialogbox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sentence;
        yield return new WaitForSeconds(3);
        dialogbox.SetActive(false);
        GetComponent<BoxCollider>().enabled = true;
    }

    //private IEnumerator DestroyMush()
    //{
    //    navmesh.isStopped = true;
    //    animator.SetBool("isfollowing", false);
    //    //Play Portal Leaving Particles
    //    yield return new WaitForSeconds(1);
    //    Destroy(this.gameObject);
    //}

}