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


    public Button AnthroTributs;
    public Button AnthroMalediction;
    public Button AnthroOs;


    public static MainManager instance;

    public GameObject pageButtons;

    public int nb_buttons_displayed = 4;
    public GameObject passwordManager;

    private List<GameObject> alreadyUsed;


    public void Start()
    {
        AlienBasesButton.onClick.AddListener(() => { OnPageOpen("Alien", "Bases"); alreadyUsed.Add(AlienBasesButton.gameObject); });
        AlienGuerreButton.onClick.AddListener(() => { OnPageOpen("Alien", "Guerre"); alreadyUsed.Add(AlienGuerreButton.gameObject); });
        AlienAbductionsButton.onClick.AddListener(() => { OnPageOpen("Alien", "Abductions"); alreadyUsed.Add(AlienAbductionsButton.gameObject); });

        PyramideGizehButton.onClick.AddListener(() => { OnPageOpen("Pyramide", "Gizeh"); alreadyUsed.Add(PyramideGizehButton.gameObject); });
        PyramideToutButton.onClick.AddListener(() => { OnPageOpen("Pyramide", "Toutânkhamon"); alreadyUsed.Add(PyramideToutButton.gameObject); });
        PyramideBosnieButton.onClick.AddListener(() => { OnPageOpen("Pyramide", "Bosnie"); alreadyUsed.Add(PyramideBosnieButton.gameObject); });

        ParanFantomeButton.onClick.AddListener(() => { OnPageOpen("Paranormal", "Fantome"); alreadyUsed.Add(ParanFantomeButton.gameObject); });
        ParaMaisonButton.onClick.AddListener(() => { OnPageOpen("Paranormal", "Maison"); alreadyUsed.Add(ParaMaisonButton.gameObject); });

        AnthroTributs.onClick.AddListener(() => { OnPageOpen("Anthropologie", "Tributs"); alreadyUsed.Add(AnthroTributs.gameObject); });

        AnthroMalediction.onClick.AddListener(() => { OnPageOpen("Anthropologie", "Malediction"); alreadyUsed.Add(AnthroMalediction.gameObject); });
        AnthroOs.onClick.AddListener(() => { OnPageOpen("Anthropologie", "Os"); alreadyUsed.Add(AnthroOs.gameObject);  });


        instance = FindFirstObjectByType<MainManager>();
        passwordManager = GameObject.FindGameObjectWithTag("passwordManager");

        DisplayOneButtonByCategory();

        alreadyUsed = new List<GameObject>();

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

        AnthroMalediction.gameObject.SetActive(false);
        AnthroOs.gameObject.SetActive(false);
        AnthroTributs.gameObject.SetActive(false);
    }

    private void ChangeDisplayedButtonsByInterest()
    {
        HideButtons();
        var sortedDict = instance.model.OrderBy(x => x.Value.interest);

        for (int i = 0; i < nb_buttons_displayed; i++)
        {
            
            var rd = Random.Range(0f, 1f);
            PageTags cat = (PageTags)(-1);

            if (rd > 0.3f)
            {
                cat = sortedDict.ElementAt(0).Key;
            }

            else
            {
                var rd2 = new System.Random();
                cat = sortedDict.ElementAt(rd2.Next(0, instance.model.Count)).Key;
            }
           
            var cont = pageButtons.transform.Find(TagToString(cat));
            var randChilds = Enumerable.Range(0, cont.childCount - 1).ToList();
            Shuffle(randChilds);

            foreach (var child in randChilds)
            {
                if(!cont.GetChild(child).gameObject.activeInHierarchy && !alreadyUsed.Contains(cont.GetChild(child).gameObject))
                {
                    cont.GetChild(child).gameObject.SetActive(true);
                    alreadyUsed.Add(cont.GetChild(child).gameObject);
                    break;
                }
                    
            }

            alreadyUsed.Clear();

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

        Debug.Log("li : " +  m_page.GetComponent<TagsList>()); ;
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
            Debug.Log("mod " + instance.model + "tag " + tag + "mt " + instance.model[tag]);
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
