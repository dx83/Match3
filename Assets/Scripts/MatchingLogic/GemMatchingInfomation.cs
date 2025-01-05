using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UIElements;

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
    /*
        1. 가로 세로 중에서 랜덤 선택
        2. 0, 3, 6 중에서 랜덤 선택
        3. 거기서 부터 무조건 왼쪽에서 오른쪽으로 검색
        4. 힌트 효과는 3초 있다가 보여주기

        무조건 스왑이동이 전제, 적 성에서 먼쪽을 선택
        ##*####
        **#**##
        ##*####
        #######
        4방향으로 이동후 가로 세로 즉시 매칭과 같은 효과
    */
    static List<List<GemInformation>> potentialMatches = new List<List<GemInformation>>();
    static GemInformation targetGem = new GemInformation();
    static public void IsMatchingForHint(List<GemInformation> gems)
    {
        if (potentialMatches.Count == 0)
        {
            for (int i = 0; i < 4; i++)
                potentialMatches.Add(new List<GemInformation>());
        }

        //int cond0 = Random.Range(0, 2);
        //int cond1 = Random.Range(0, 3);
        //switch(cond0)
        //{
        //    case 0:     break;
        //
        //}

        // 이동후 해당 가로줄 세로줄 전부 체크
        for (int i = 0; i < gems.Count; i++) // 모든 젬 대상
        {
            targetGem.color = gems[i].color;
            targetGem.coord = gems[i].coord;
            // 검수시 마지막은 항상 타겟젬을 검사 해야되는디...
            // left
            if ((i % Constants.COLUMNS) != 0)
            {
                int max = i - targetGem.coord.x * Constants.COLUMNS;

                for (int j = 0; j < max; j++)     // 체크할 젬이 속해있는 행 검수
                {
                    int current = targetGem.coord.x * Constants.COLUMNS + j;
                    potentialMatches[j].Add(gems[current]);
                    
                    for (int k = 0; k < max; k++)                   // 매칭 확인
                    {
                        int next = current + k + 1;
                        if (gems[current].isSameColor(gems[next]))  // 같은 젬이 연속되는지 확인
                        {
                            potentialMatches[j].Add(gems[next]);
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (potentialMatches[j].Count < 3)    // 2개 이하인 경우 리스트 비움
                        potentialMatches[j].Clear();
                }
                //for (int j = 0; j < Constants.COLUMNS - 2; j++)
                //{
                //    int current = i * Constants.COLUMNS + j;
                //    potentialMatches[j].Add(gems[current]);
                //
                //    for (int k = 0; k < Constants.COLUMNS; k++)     // 매칭 확인
                //    {
                //        int next = current + k + 1;
                //        if (gems[current].isSameColor(gems[next]))  // 같은 젬이 연속되는지 확인
                //        {
                //            rawMatches[j].Add(gems[next]);
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }
                //
                //    if (rawMatches[j].Count < 3)    // 2개 이하인 경우 리스트 비움
                //        rawMatches[j].Clear();
                //}
                //
                //for (int j = 0; j < Constants.COLUMNS - 2; j++)     // 현재 행의 각 젬에서
                //{
                //    foreach (GemInformation e in rawMatches[j])     // 매칭되어 파괴되는 젬 모두 표시
                //        e.isDestroy = true;
                //    rawMatches[j].Clear();          // 다음 행을 위해 리스트 비움
                //}
            }
        }
    }

    static public IEnumerator FlickerGemEffect(List<GemInformation> potentialMatches)   // 젬 깜박임 효과
    {
        List<GemInformation> temp = potentialMatches.FindAll(e => e.coord.x == 3).ToList();
        while(true)
        {
            for (float i = 0.95f; i >= 0.55f; i -= 0.1f)
            {
                // 힌트에 해당되는 각 젬마다
                foreach (var item in temp)//potentialMatches)
                {
                    if (item == null)
                        yield break;
                    // 알파값을 1f 에서 0.3f 까지 수정 : 점점 투명해짐
                    Color c = item.gemImg.color;
                    c.a = i;
                    item.gemImg.color = c;
                }
                // 지정한 수치만큼 딜레이
                yield return new WaitForSeconds(0.15f);
            }
            for (float i = 0.55f; i <= 0.95f; i += 0.1f)
            {
                // 힌트에 해당되는 각 젬마다
                foreach (var item in temp)//potentialMatches)
                {
                    if (item == null)
                        yield break;
                    // 알파값을 0.3f 에서 1f 까지 수정 : 점점 다시 돌아옴
                    Color c = item.gemImg.color;
                    c.a = i;
                    item.gemImg.color = c;
                }
                // 지정한 수치만큼 딜레이
                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    // 즉시 매칭 찾기
    static List<List<GemInformation>> rawMatches = new List<List<GemInformation>>();
    static List<List<GemInformation>> colMatches = new List<List<GemInformation>>();
    static public void IsMatchingTheGem(List<GemInformation> gems)
    {
        if (rawMatches.Count == 0)
        {
            for (int i = 0; i < Constants.COLUMNS - 2; i++)
                rawMatches.Add(new List<GemInformation>()); // 한 행의 각 젬 매칭 리스트
            for (int i = 0; i < Constants.RAWS - 2; i++)
                colMatches.Add(new List<GemInformation>()); // 한 열의 각 젬 매칭 리스트
        }

        for (int i = 0; i < Constants.RAWS; i++)                // 한 행씩
        {
            for (int j = 0; j < Constants.COLUMNS - 2; j++)     // 각 젬마다
            {
                int current = i * Constants.COLUMNS + j;
                rawMatches[j].Add(gems[current]);
                
                for (int k = 0; k < Constants.COLUMNS; k++)     // 매칭 확인
                {
                    int next = current + k + 1;
                    if (gems[current].isSameColor(gems[next]))  // 같은 젬이 연속되는지 확인
                    {
                        rawMatches[j].Add(gems[next]);
                    }
                    else
                    {
                        break;
                    }
                }
                
                if (rawMatches[j].Count < 3)    // 2개 이하인 경우 리스트 비움
                    rawMatches[j].Clear();
            }

            for (int j = 0; j < Constants.COLUMNS - 2; j++)     // 현재 행의 각 젬에서
            {
                foreach (GemInformation e in rawMatches[j])     // 매칭되어 파괴되는 젬 모두 표시
                    e.isDestroy = true;
                rawMatches[j].Clear();          // 다음 행을 위해 리스트 비움
            }
        }

        for (int i = 0; i < Constants.COLUMNS; i++)             // 한 열씩
        {
            for (int j = 0; j < Constants.RAWS - 2; j++)        // 각 젬마다
            {
                int current = i + (j * Constants.RAWS);
                colMatches[j].Add(gems[current]);

                for (int k = 0; k < Constants.RAWS; k++)        // 매칭 확인
                {
                    int next = current + (k + 1) * Constants.RAWS;
                    if (gems[current].isSameColor(gems[next]))  // 같은 젬이 연속되는지 확인
                    {
                        colMatches[j].Add(gems[next]);
                    }
                    else
                    {
                        break;
                    }
                }

                if (colMatches[j].Count < 3)    // 2개 이하인 경우 리스트 비움
                    colMatches[j].Clear();
            }

            for (int j = 0; j < Constants.RAWS - 2; j++)        // 현재 열의 각 젬에서
            {
                foreach (GemInformation e in colMatches[j])     // 매칭되어 파괴되는 젬 모두 표시
                    e.isDestroy = true;
                colMatches[j].Clear();          // 다음 열을 위해 리스트 비움
            }
        }
    }

    static public void Test(List<GemInformation> gems) // 파괴
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
