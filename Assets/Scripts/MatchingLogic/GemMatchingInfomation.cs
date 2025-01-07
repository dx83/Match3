using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GemMatchingInformation
{
    // ��� ��Ī ã��
    static List<List<GemInformation>> rawMatches = new List<List<GemInformation>>();
    static List<List<GemInformation>> colMatches = new List<List<GemInformation>>();
    static public void IsMatchingTheGem(List<GemInformation> gems)
    {
        if (rawMatches.Count == 0)
        {
            for (int i = 0; i < Constants.COLUMNS - 2; i++)
                rawMatches.Add(new List<GemInformation>()); // �� ���� �� �� ��Ī ����Ʈ
            for (int i = 0; i < Constants.RAWS - 2; i++)
                colMatches.Add(new List<GemInformation>()); // �� ���� �� �� ��Ī ����Ʈ
        }

        for (int i = 0; i < Constants.RAWS; i++)                // �� �྿
        {
            for (int j = 0; j < Constants.COLUMNS - 2; j++)     // �� ������
            {
                int current = i * Constants.COLUMNS + j;
                rawMatches[j].Add(gems[current]);
                
                for (int k = 0; k < Constants.COLUMNS; k++)     // ��Ī Ȯ��
                {
                    int next = current + k + 1;
                    if (gems[current].isSameColor(gems[next]))  // ���� ���� ���ӵǴ��� Ȯ��
                    {
                        rawMatches[j].Add(gems[next]);
                    }
                    else
                    {
                        break;
                    }
                }
                
                if (rawMatches[j].Count < 3)    // 2�� ������ ��� ����Ʈ ���
                    rawMatches[j].Clear();
            }

            for (int j = 0; j < Constants.COLUMNS - 2; j++)     // ���� ���� �� ������
            {
                foreach (GemInformation e in rawMatches[j])     // ��Ī�Ǿ� �ı��Ǵ� �� ��� ǥ��
                    e.Destroy();
                rawMatches[j].Clear();          // ���� ���� ���� ����Ʈ ���
            }
        }

        for (int i = 0; i < Constants.COLUMNS; i++)             // �� ����
        {
            for (int j = 0; j < Constants.RAWS - 2; j++)        // �� ������
            {
                int current = i + (j * Constants.RAWS);
                colMatches[j].Add(gems[current]);

                for (int k = 0; k < Constants.RAWS; k++)        // ��Ī Ȯ��
                {
                    int next = current + (k + 1) * Constants.RAWS;
                    if (gems[current].isSameColor(gems[next]))  // ���� ���� ���ӵǴ��� Ȯ��
                    {
                        colMatches[j].Add(gems[next]);
                    }
                    else
                    {
                        break;
                    }
                }

                if (colMatches[j].Count < 3)    // 2�� ������ ��� ����Ʈ ���
                    colMatches[j].Clear();
            }

            for (int j = 0; j < Constants.RAWS - 2; j++)        // ���� ���� �� ������
            {
                foreach (GemInformation e in colMatches[j])     // ��Ī�Ǿ� �ı��Ǵ� �� ��� ǥ��
                    e.Destroy();
                colMatches[j].Clear();          // ���� ���� ���� ����Ʈ ���
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

    // �ش� ���� �����ϴ� ��� ��Ī�� �ִ��� Ȯ��
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

    // ������ �Է¹޾� �ش� �������� �������� ���� ��Ī�� �ִ��� �˻��ϴ� �Լ� ȣ��
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

    // ������ ��Ī�Ǵ� �ش� ���� ��Ī�Ǵ��� Ȯ��
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

    static public IEnumerator FlickerGemEffect(List<GemInformation> potentialMatches)   // �� ������ ȿ��
    {
        List<GemInformation> temp = potentialMatches.FindAll(e => e.coord.x == 3).ToList();
        while (true)
        {
            for (float i = 0.95f; i >= 0.55f; i -= 0.1f)
            {
                // ��Ʈ�� �ش�Ǵ� �� ������
                foreach (var item in temp)//potentialMatches)
                {
                    if (item == null)
                        yield break;
                    // ���İ��� 1f ���� 0.3f ���� ���� : ���� ��������
                    Color c = item.gemImg.color;
                    c.a = i;
                    item.gemImg.color = c;
                }
                // ������ ��ġ��ŭ ������
                yield return new WaitForSeconds(0.15f);
            }
            for (float i = 0.55f; i <= 0.95f; i += 0.1f)
            {
                // ��Ʈ�� �ش�Ǵ� �� ������
                foreach (var item in temp)//potentialMatches)
                {
                    if (item == null)
                        yield break;
                    // ���İ��� 0.3f ���� 1f ���� ���� : ���� �ٽ� ���ƿ�
                    Color c = item.gemImg.color;
                    c.a = i;
                    item.gemImg.color = c;
                }
                // ������ ��ġ��ŭ ������
                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    static public void Test(List<GemInformation> gems) // �׽�Ʈ �ı�
    {
        for (int i = 0; i < Constants.COLUMNS; i++)
            gems[i].Test();
    }
}
