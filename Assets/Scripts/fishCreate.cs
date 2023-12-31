using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class fishCreate : MonoBehaviour
{        
    [SerializeField] private GameObject fishParent; // Balıkların çekileceği prefab
    [SerializeField] private GameObject pathParent; // Balıkların gideceği pathler

    [SerializeField] public  int fishCount; // Oyunda toplam oluşacak balık sayısı
    private List<bool> pathLock; // Pathlerin durumu
    private List<GameObject> fishes; // Balıkların kontrol altına alınacağı dizi
    private List<GameObject> lastPath; // Balıkların son gideceği durak
    private List<float> moveSpeed; // Balıkların rastgele oluşacağı hız bilgisi
    public static bool play;

    public static bool speedSlow;
    [SerializeField] private  float speedTime;

    public static bool pointBoost;
    [SerializeField] private float pointTime;


    [SerializeField] private GameObject specialParent;
    [SerializeField] private bool specialOn;
    [SerializeField] private bool speciaComing;

    [SerializeField] private GameObject stray;

    [SerializeField] private GameObject roket;

    public bool fishCreating = false;

    

    int fishIndex;
        
    
    
    void Start()
    {
        
        play = false;
        speedTime = 10f;
        pointTime = 10f;
        speciaComing = true;
        set();


    }

  
    void Update()
    {
        specialControl();
        if(fishCreating)
        {
          
            fishSpam(); 
        }
        
        if(roketSkill.roketGeliyor)
        {
            
        }else
        {
            
            
                
            if(speedSlow && play )
            {
                if(speedTime<= 0)
                {
                    speedSlow = false;
                    speedTime = 10;
                    for(int i = 0; i< stray.transform.childCount; i++)
                    {
                        if(stray.transform.GetChild(i).name == "speedBoost")
                        {
                            Destroy(stray.transform.GetChild(i).gameObject);
                        }
                    }
                }else
                {
                    speedTime -= Time.deltaTime;
                    
                        
                        
                }
            }

            if(pointBoost && play )
            {
                if(pointTime<= 0)
                {
                    pointBoost = false;
                    pointTime = 10;
                    for(int i = 0; i< stray.transform.childCount; i++)
                    {
                        if(stray.transform.GetChild(i).name == "pointBoost")
                        {
                            Destroy(stray.transform.GetChild(i).gameObject);
                        }
                    }
                }else
                {
                    pointTime -= Time.deltaTime;
                    
                        
                        
                }
            }
                
            if(fishCreating)
            {
                move();
            }
            
            
           
        }

        
        
       
    }

    
    
    // Bu kısım, oyun içinde üretilecek balıkların temel verilerini içerir.
    void set()
    {
        
        pathLock = new List<bool>();
        fishes = new List<GameObject>();
        lastPath = new List<GameObject>();
        moveSpeed = new List<float>();
        Fishing(fishCount);
    }

    void Fishing(int count)
    {
        for(int i = 0; i<count; i++)
        {
            pathLock.Add(false);
            fishes.Add(null);
            lastPath.Add(null);
            moveSpeed.Add(0);
            
        }
        fishCreating = true;
    }

    void fishSpam(){
        
        
        for (int i = 0; i < fishCount; i++)
        {
            
            if(!pathLock[i]){
                
                

                moveSpeed[i] =  Random.Range(0.01f, eventClass.level*0.2f*Time.deltaTime);
                //Rastgele balık oluşturuluyor
                
                
                
                if(specialOn)
                {
                    fishIndex = Random.Range(0, specialParent.transform.childCount);
                    fishes[i] = null;
                    fishes[i] = Instantiate(specialParent.transform.GetChild(fishIndex).gameObject);
                    fishes[i].name = specialParent.transform.GetChild(fishIndex).name;
                    
                    if(fishIndex == 3)
                    {
                        moveSpeed[i] = moveSpeed[i]*1.5f;
                    }
                    
                    specialOn = false;

                    
                }else
                {
                    fishIndex = Random.Range(0, 8);
                    fishes[i] = null;
                    fishes[i] = Instantiate(fishParent.transform.GetChild(fishIndex).gameObject);
                    fishes[i].name = fishParent.transform.GetChild(fishIndex).name;
                    
                    
                }

                
                setFish(fishes[i], i);
                
                //Yol belirle;
                setPath();
                fishes[i].transform.position = pathParent.transform.GetChild(path1).transform.position;
                lastPath[i] = pathParent.transform.GetChild(path2).gameObject;
                
                


                
    

                
                
                // Yolu kilitle
                pathLock[i] = true;
            }
        }
    }

    string fishScore;
    string earnFishScore(string s, int i)
    {
        
        fishScore = (Mathf.Round((moveSpeed[i] * 1000 + eventClass.level*2.5f))).ToString();
        return fishScore;
        
    }



    int path1;
    int path2;
    void setPath()
    {
        if(Random.Range(0, 8)<4)
        {
            path1 = Random.Range(0,4);
            path2 = Random.Range(4,8);
        }else
        {
            path1 = Random.Range(4,8);
            path2 = Random.Range(0,4);
        }
    }


    void setFish(GameObject fish, int i)
    {
        // Balığın rengini belirle ve score ata
        GameObject color = new GameObject(fish.name);
        GameObject score = new GameObject(earnFishScore(fish.name, i));
            
        color.transform.parent = fish.transform;
        score.transform.parent = fish.transform;

        //Balığı eşsiz kıl
        fish.name = System.Guid.NewGuid().ToString();
    }


    void move()
    {
        for(int i = 0; i<fishCount; i++)
        {
            
            //pointTemp = fishes[i].transform.GetChild(1).gameObject.name;
            
            if(fishes[i] != null)
            {
                if(pathLock[i])
                {
                    
                    
                    if(distance( fishes[i].transform.position, lastPath[i].transform.position) || fishes[i] == null)
                    {

                        
                        pathLock[i] = delete(fishes[i]);
                        
                    }else
                    {
                        speedTemp = moveSpeed[i];
                        bonusControl(moveSpeed[i], i);
                        fishes[i].transform.position = Vector2.MoveTowards(fishes[i].transform.position, lastPath[i].transform.position, moveSpeed[i]);
                        moveSpeed[i] = speedTemp;
                    //fishes[i].transform.Translate(lastPath[i].transform.position* Time.deltaTime * moveSpeed[i]);
                    }
                    
                    
                }
            }else
            {
                
                pathLock[i] = false;
            }
        }
    }

    bool distance(Vector2 a, Vector2 b){

        if(Vector2.Distance(a, b)<2f){
            b = a;
            return true;
        }else{
            return false;
        }
    
    }

    bool delete(GameObject a){
        
        Destroy(a);

        return false;

    }

    int tempLevel;
    void specialControl()
    {
        if(eventClass.level % 1 == 0)
        {
            if(!speciaComing)
            {
                tempLevel = eventClass.level;
                specialOn = true;
                        
            }
            
            
            speciaComing = true;
                    
        }
        
           if(eventClass.level != tempLevel)
           {
                speciaComing = false;
           }
           
            
        

        if(eventClass.level % 5 == 0)
        {
            if(!levelCome)
            {
                fishCreating = false;
                Debug.Log($"balık sayısı: {fishes.Count} hiz sayisi: {moveSpeed.Count} ");
                Debug.Log($"yol sayısı: {lastPath.Count} kilit sayisi: {pathLock.Count} ");
                fishCreating = false;
                fishCount +=2;
                Fishing(2);
                
                levelCome = true;

            }
            

        }else
        {
            levelCome = false;
        }
    }
    bool levelCome = false;

    
    
    string pointTemp;
    float speedTemp;
    
    void bonusControl(float speed, int i)
    {
        
        

        


    

        if(speedSlow)
        {
            
            moveSpeed[i] = speed/2;

        }
        
    }

    
}
