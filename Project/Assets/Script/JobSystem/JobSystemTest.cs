using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public class JobSystemTest : MonoBehaviour
{
    private JobHandle jobHandle;
    private NativeArray<int> resultArr;
    // Start is called before the first frame update
    void Start()
    {
        // ʵ��һ�� NativeArray
        // ���� 1�����鳤��
        // ���� 2�����ٵ��ڴ�ռ�����
        resultArr = new NativeArray<int>(1, Allocator.Persistent);

        // ʵ����һ�� job
        CalculateAdd add = new CalculateAdd()
        {
            numberA = 1,
            numberB = 2,
            resultArr = resultArr,
            result = 0,
        };

        // ͨ�� Schedule �������� Job ��ִ�У����Զ����õ� Job �� Execute() ����
        // Schedule ��������һ�� JobHandle ��ʵ��
        jobHandle = add.Schedule();
        // �ж� Job �Ƿ�ִ�����
        if (jobHandle.IsCompleted)
        {
            Debug.LogError("jobHandle Complete 1");
        }


        if (jobHandle.IsCompleted)
        {
            Debug.LogError("jobHandle Complete 2");
        }
        // ���ȣ�Schedule��һ��Job�ǱȽϼ򵥵ģ�ֻ��Ҫ����Schedule()�����Ϳ����ˡ�����Ƚ�����˼����Complete()������
        // ��������Ҫ��ȡִ�н��֮ǰ��Ҫ����Complete()����������Complete()��һ����Schedule()֮���������ã�
        // Ҳ��һ���ڵ�ǰ֡������ã�Ҳ����˵һ��Job������Update()���ƿ��Կ�֡���С���һ��Job��Ҫ��֡���е�ʱ��
        // ������Ҫʹ��IsCompleted�������ж�Job�Ƿ�ִ�����

        // �ڴ˴����� Complete()�������������̵߳ȴ� Job ִ�н������Ż����ִ������Ĵ���
        jobHandle.Complete();
        // ��ȡ Job ִ�еĽ��,���Եõ���ȷ���

        // ǿ������ʹIsCompleted����true���ڻ�ȡ���ǮҲ����Ҫ����Complete()����
        Debug.LogError("CalculateAdd resultArr:" + add.resultArr[0]);
        // �ṹ�������� resultArr��ͨ�� resultArr Ҳ�ǿ��Ի�ȡ����
        Debug.LogError("resultArr :" + resultArr[0]);
        // add.result ��ȡ��һЩ�汾�� Unity �»ᱨ��
        // �� Unity2023.3.26�汾���ԣ�û�б������ؽ���� 0,Ҳ���� result �ĸ�ֵ�����ᱻ���� 
        Debug.LogError("result:" + add.result);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        // NativeContainer����Ҫ��ʽ�����ڴ��
        if (resultArr.IsCreated) // �����������Ҫ�ֶ�����
        {
            resultArr.Dispose();
        }

    }
}
