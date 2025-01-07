using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GemMatchingInformation
{
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
                    e.Destroy();
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
                    e.Destroy();
                colMatches[j].Clear();          // 다음 열을 위해 리스트 비움
            }
        }

        foreach (var list in rawMatches)
            list.Clear();
        foreach (var list in colMatches)
            list.Clear();
    }

    static List<List<GemInformation>> potentialMatches = new List<List<GemInformation>>();
    static List<GemInformation> mergeList = new List<GemInformation>();
    static List<GemInformation> matchingList = new List<GemInformation>();
    static bool isMatching;

    static public List<GemInformation> SearchForHint(List<GemInformation> gems)
    {
        isMatching = false;

        for (int i = 0; i < gems.Count; i++)
        {
            SearchForTarget(gems, gems[i]);
            if (isMatching)
                break;
        }

        for (int i = 0; i < potentialMatches.Count; i++)
        {
            if (potentialMatches[i].Count > 0)
                matchingList.AddRange(potentialMatches[i]);
        }

        return matchingList;
    }

    // 해당 젬이 스왑하는 경우 매칭이 있는지 확인
    static void SearchForTarget(List<GemInformation> gems, GemInformation target)
    {
        if (potentialMatches.Count == 0)
        {
            for (int i = 0; i < 4; i++)
                potentialMatches.Add(new List<GemInformation>());
        }

        target.DetermineIndex();

        for (int i = 0; i < 4; i++)
        {
            SearchForMatching(gems, target, (Direction)i);
            if (potentialMatches[i].Count > 2)
            {
                isMatching = true;
                return;
            }
        }
        
        for (int i = 0; i < potentialMatches.Count; i++)
        {
            foreach (GemInformation e in potentialMatches[i])
                Debug.Log(e.DetermineIndex());
        }
    }

    // 방향을 입력받아 해당 방향으로 스왑했을 때의 매칭이 있는지 검사하는 함수 호출
    static void SearchForMatching(List<GemInformation> gems, GemInformation target, Direction search)
    {
        int swapIdx = target.fourIdx[(int)search];

        Direction lineA = 0;
        Direction lineB = 0;

        switch (search)
        {
            case Direction.Left:
            case Direction.Right:
                lineA = Direction.Up;
                lineB = Direction.Down;
                break;
            case Direction.Up:
            case Direction.Down:
                lineA = Direction.Left;
                lineB = Direction.Right;
                break;
            default:
                return;
        }

        SearchSingleDirection(gems, target, swapIdx, search);
        if (matchingList.Count < 3)
            matchingList.Clear();
        else
            potentialMatches[(int)search].AddRange(matchingList);

        SearchSingleDirection(gems, target, swapIdx, lineA);
        mergeList = matchingList.ToList();
        SearchSingleDirection(gems, target, swapIdx, lineB);
        mergeList.AddRange(matchingList);

        if (mergeList.Count < 3)
            mergeList.Clear();
        else
            potentialMatches[(int)search].AddRange(mergeList);

        mergeList.Clear();
    }

    // 스왑후 매칭되는 해당 젬과 매칭되는지 확인
    static void SearchSingleDirection(List<GemInformation> gems, GemInformation target, int index, Direction direction)
    {
        matchingList.Clear();

        int start = 0;
        int loop = 0;
        int sign = 0;
        switch(direction)
        {
            case Direction.Left:
                {
                    start = index - 1;
                    loop = index % Constants.COLUMNS;
                    sign = -1;
                    break;
                }
            case Direction.Right:
                {
                    start = index + 1;
                    loop = Constants.COLUMNS - index % Constants.COLUMNS - 1;
                    sign = 1;
                    break;
                }
            case Direction.Up:
                {
                    start = index - Constants.COLUMNS;
                    loop = index / Constants.COLUMNS;
                    sign = -1 * Constants.COLUMNS;
                    break;
                }
            case Direction.Down:
                {
                    start = index + Constants.COLUMNS;
                    loop = Constants.COLUMNS - index / Constants.COLUMNS - 1;
                    sign = 1 * Constants.COLUMNS;
                    break;
                }
            default:
                return;
        }

        if (start < 0 || start >= gems.Count)
            return;

        for (int i = 0; i < loop; i++)
        {
            if (target.isSameColor(gems[start + (i * sign)]))
            {
                matchingList.Add(gems[i]);
            }
            else
                break;
        }
    }

    static public IEnumerator FlickerGemEffect(List<GemInformation> potentialMatches)   // 젬 깜박임 효과
    {
        List<GemInformation> temp = potentialMatches.FindAll(e => e.coord.x == 3).ToList();
        while (true)
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

    static public void Test(List<GemInformation> gems) // 테스트 파괴
    {
        for (int i = 0; i < Constants.COLUMNS; i++)
            gems[i].Test();
    }
}
