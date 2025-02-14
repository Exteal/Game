using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using Random = UnityEngine.Random;
using TMPro;

public class PageScript : MonoBehaviour
{
    public Button AlienBasesButton;
    public Button AlienGuerreButton;
    public Button AlienAbductionsButton;


    public Button PyramideGizehButton;
    public Button PyramideToutButton;
    public Button PyramideBosnieButton;


    public Button ParanFantomeButton;
    public Button ParaMaisonButton;

    public static MainManager instance;

    public GameObject pageButtons;

    private int changeButtonsDelay = 2;

    public int nb_buttons_displayed = 3;
    public GameObject passwordManager;


    public void Start()
    {
        AlienBasesButton.onClick.AddListener(() => { OnPageOpen("Alien", "Bases"); });
        AlienGuerreButton.onClick.AddListener(() => { OnPageOpen("Alien", "Guerre");  });
        AlienAbductionsButton.onClick.AddListener(() => { OnPageOpen("Alien", "Abductions"); });

        PyramideGizehButton.onClick.AddListener(() => { OnPageOpen("Pyramide", "Gizeh"); });
        PyramideToutButton.onClick.AddListener(() => { OnPageOpen("Pyramide", "Toutânkhamon"); });
        PyramideBosnieButton.onClick.AddListener(() => { OnPageOpen("Pyramide", "Bosnie"); });

        ParanFantomeButton.onClick.AddListener(() => { OnPageOpen("Paranormal", "Fantome"); });
        ParaMaisonButton.onClick.AddListener(() => { OnPageOpen("Paranormal", "Maison"); });

        instance = FindFirstObjectByType<MainManager>();
        passwordManager = GameObject.FindGameObjectWithTag("passwordManager");

        DisplayOneButtonByCategory();

    }


    private void DisplayOneButtonByCategory()
    {
        HideButtons();

        for (int i = 0; i < pageButtons.transform.childCount; i++)
        {
            var rd = Random.Range(0, pageButtons.transform.GetChild(i).childCount);
            pageButtons.transform.GetChild(i).GetChild(rd).gameObject.SetActive(true);
            
        }
    }

    private void ClosePage()
    {
        foreach(Transform categ in transform)
        {
            foreach(Transform page in categ.transform)
            {
                page.gameObject.SetActive(false);
            }
            
        } 
    }

    private void HideButtons()
    {
        AlienAbductionsButton.gameObject.SetActive(false);
        AlienBasesButton.gameObject.SetActive(false);
        AlienGuerreButton.gameObject.SetActive(false);

        PyramideGizehButton.gameObject.SetActive(false);
        PyramideToutButton.gameObject.SetActive(false);
        PyramideBosnieButton.gameObject.SetActive(false);

        ParanFantomeButton.gameObject.SetActive(false);
        ParaMaisonButton.gameObject.SetActive(false);
    }

    private void ChangeDisplayedButtonsByInterest()
    {
        HideButtons();
        var sortedDict = instance.model.OrderBy(x => x.Value.interest);

        for (int i = 0; i < nb_buttons_displayed; i++)
        {
            
            var rd = Random.Range(0f, 1f);

            //Debug.Log("Button " + i + " rd " + rd);


            float acc = 0;
            PageTags cat = (PageTags)(-1) ;

            for (int j = 0; j < sortedDict.Count(); j++)
            {
               // Debug.Log("Jiyi " + j);
                var inte = sortedDict.ElementAt(j).Value.interest;

                acc += (inte - sortedDict.ElementAt(0).Value.interest) / sortedDict.ElementAt(sortedDict.Count() - 1).Value.interest;

               // Debug.Log("acc : " + acc);
                if (acc > rd)
                {
                    cat = sortedDict.ElementAt(j).Key;
                   // Debug.Log("break");

                    break;
                }
            }

            // Debug.Log("conffrfrterdddd");
            
            var cont = pageButtons.transform.Find(TagToString(cat));

           // Debug.Log("cont : cat" +cont + " : " + cat);

            var randChilds = Enumerable.Range(0, cont.childCount - 1).ToList();
            Shuffle(randChilds);

          //  Debug.Log("Order : " + randChilds.ToString());
            foreach (var child in randChilds)
            {
                if(!cont.GetChild(child).gameObject.activeInHierarchy)
                {
                    cont.GetChild(child).gameObject.SetActive(true);
                  //  Debug.Log("Selected : " + child);
                    break;
                }
                    
            }
        }   
    }
    private void Shuffle(IList<int> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public void OnPageOpen(string categ, string page)
    {
        ClosePage();

        var m_page = transform.Find(categ).Find(page).gameObject;
        m_page.SetActive(true);
        
        if (new System.Random().Next(1, 101) > 30)
        {
            StartCoroutine(DisplayPartial(m_page));     
        }

        UpdateInterest(m_page.GetComponent<TagsList>());
       
        ChangeDisplayedButtonsByInterest();
    }

    private IEnumerator DisplayPartial(GameObject page)
    {

        var text_cp = page.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();
        var text = text_cp.text;

        text_cp.text = passwordManager.GetComponent<PasswordScript>().partialPassword();

        yield return new WaitForSeconds(2);

        text_cp.text = text;
    }

    private void UpdateInterest(TagsList tagsList)
    {
        foreach (var tag in tagsList.tags)
        {
            var data = instance.model[tag];
            var updated = new PlayerData(data.interest + 0.3f, data.stress);
            instance.model[tag] = updated;
        }
    }

    private string TagToString(PageTags tag)
    {
        return Enum.GetName(typeof(PageTags), tag);
    }
}
