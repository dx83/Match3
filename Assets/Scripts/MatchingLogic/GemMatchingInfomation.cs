using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GemMatchingInfomation
{
    static public void SearchForMatching(List<GemInformation> gems, GemColors[] favorColors, bool isPlayer = true)
    {
        bool Loop = true;
        //int count = 0;
        while (Loop)
        {
            Loop = false;

            for (int i = 0; i < Constants.RAWS; i++)
            {

            }
        }
    }

    static public void SearchForMatching(List<GemInformation> gems)
    {
        bool Loop = true;
        //int count = 0;
        while (Loop)
        {
            Loop = false;

            
        }

        for (int i = 0; i < Constants.RAWS; i++)
        {
            for (int j = 0; j < Constants.COLUMNS; j++)
            {
                GemInformation g = gems[(i * 7) + j];

                if (g.coord.x - 1 > 0)
                {

                }
            }
        }
    }

    //static List<GemInformation> concat = new List<GemInformation>();
    static List<List<GemInformation>> matches = new List<List<GemInformation>>();
    static public void IsMatchingTheGem(List<GemInformation> gems)
    {
        for (int i = 0; i < Constants.RAWS; i++)
        {
            GemInformation[] g = gems.FindAll(e => e.coord.x == i).OrderBy(e => e.coord.y).ToArray(); ;

            for (int j = 0; j < Constants.COLUMNS; j++)
            {
                matches.Add(new List<GemInformation>());    // 최적화 필요
                matches[j].Add(g[j]);

                for (int k = j + 1; k < Constants.COLUMNS; k++)
                {
                    if (g[j].isSameColor(g[k]))
                    {
                        matches[j].Add(g[k]);
                    }
                    else
                    {
                        break;
                    }
                }

                if (matches[j].Count < 3)
                    matches[j].Clear();
            }

            for (int j = 0; j < Constants.COLUMNS; j++)
            {
                foreach (GemInformation e in matches[j])
                    e.isDestroy = true;
            }
        }
        /*
            한 행씩 : 매칭이 이루어지는 젬들을 담을 리스트
            한 젬씩 함수( 다음 젬과 색이 맞으면 리스트에 담기 => 다른색 나오면 for문 종료 => 리스트에 젬이 3개 미만이면 리스트 비움 => 종료)
            리스트 중복 제거하고 그 젬들에 isDestroy = true 하면 될듯

            한행씩만 해보까???
         */
    }

    static public void Test(List<GemInformation> gems)
    {
        for (int i = 0; i < Constants.COLUMNS; i++)
            gems[i].Test();
    }
}

/*  ### 매칭 찾기 (힌트, !! 매칭되는게 없다면 새판 짜기 !!)
    
    스왑 대상 선정
    
    플레이어의 경우 : 랜덤으로 보여줌
    1. 가로 세로 중에서 랜덤 선택
    2. 0, 3, 6 중에서 랜덤 선택
    3. 거기서 부터 무조건 오른쪽으로 검색
    4. 힌트 효과는 3초 있다가 보여주기
    
    AI의 경우 : 유리한 색으로 매칭 실행하도록 유도
    1. 위에서 부터 검색해서 가장 유리한 색 선택
    2. 선호색이 없다면 가장 위에꺼 선택 (배열에서 가장 처음에 있는 것)

    매칭되는게 하나도 없다면 GameManager에서 새판 짜기 실행 -> 힌트 찾기

    ### 실제 스왑 후 매칭 검수 함수 -> 젬 파괴, 젬 생성, 젬 채우기
*/
