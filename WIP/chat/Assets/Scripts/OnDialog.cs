using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnDialog : MonoBehaviour
{
    int talkCnt = 10;       // 대화의 총 갯수를 설정해줍니다.
    int strCnt = 0;         // 이 변수가 하나씩 커져가면서 대화를 진행합니다.
    int numOfChar = 2;     // 총 등장인물의 수입니다.
    string[] talk;          // 대화 내용을 저장할 공간입니다.
    public Text txt;        // Text 오브젝트에 접근하기
    public Text inform;
    public Image[] charactors;
    public Image showText;
    int[] character;

    // Use this for initialization
    void Start()
    {
            talk = new string[talkCnt]; // 대화 저장공간 초기화
            character = new int[talkCnt];
            txt = GameObject.Find("Canvas").transform.Find("Text").GetComponent<Text>();
            showText = GameObject.Find("Canvas").transform.Find("talkScreen").GetComponent<Image>();

            inform = GameObject.Find("Canvas").transform.Find("Inform").GetComponent<Text>();
            inform.gameObject.SetActive(false);

            charactors = new Image[numOfChar];  // 이미지 호출 배열
            charactors[0] = GameObject.Find("Canvas").transform.Find("char").GetComponent<Image>();  // 우측 캐릭터
            charactors[1] = GameObject.Find("Canvas").transform.Find("char2").GetComponent<Image>();  // 좌측 캐릭터

            Talk();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Jump"))
        {
            strCnt++;
            // '엔터'나 '스페이스바'로 다음 대화로 넘기기
        }
        if (strCnt < talkCnt)
        {
            showAll();
        }
        else
        {
            showText.gameObject.SetActive(false);
            txt.gameObject.SetActive(false);
            for (int i = 0; i < numOfChar; i++)
            {
                charactors[i].gameObject.SetActive(false);      // 모든 캐릭터 이미지 비활성화
            }
        }
    }

    // 화면에 캐릭터와 내용을 출력시키기 위한 함수
    void showAll()
    {
        showText.gameObject.SetActive(true);
        txt.gameObject.SetActive(true);
        for (int i = 0; i < numOfChar; i++)
        {
            charactors[i].gameObject.SetActive(false);      // 모든 캐릭터 이미지 비활성화
        }
        charactors[character[strCnt]].gameObject.SetActive(true);
        // 대화하는 캐릭터 활성화하여 출력
        txt.text = talk[strCnt];
        // 대화내용 출력
    }

    // 대화 내용과 캐릭터 설정
    void Talk()
    {
        // 대화 내용
        talk[0] = "샘플 텍스트.";
        talk[1] = "스왑 텍스트입니다.";
        talk[2] = "새앰플";
        talk[3] = "텍스트으";
        talk[4] = "샘플-";
        talk[5] = "텍스트-?";
        talk[6] = "샘!";
        talk[7] = "플!";
        talk[8] = "텍스트!";
        talk[9] = "체크";

        // 캐릭터 등장 순서 설정
        character[0] = 0; //charactors[0]
        character[1] = 1; //charactors[1]
        character[2] = 0;
        character[3] = 0;
        character[4] = 1;
        character[5] = 1;
        character[6] = 0;
        character[7] = 0;
        character[8] = 0;
        character[9] = 1;
    }
}