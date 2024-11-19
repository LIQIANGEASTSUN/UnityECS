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
        // 实例一个 NativeArray
        // 参数 1：数组长度
        // 参数 2：开辟的内存空间类型
        resultArr = new NativeArray<int>(1, Allocator.Persistent);

        // 实例化一个 job
        CalculateAdd add = new CalculateAdd()
        {
            numberA = 1,
            numberB = 2,
            resultArr = resultArr,
            result = 0,
        };

        // 通过 Schedule 方法调用 Job 的执行，会自动调用到 Job 的 Execute() 方法
        // Schedule 方法返回一个 JobHandle 的实例
        jobHandle = add.Schedule();
        // 判断 Job 是否执行完毕
        if (jobHandle.IsCompleted)
        {
            Debug.LogError("jobHandle Complete 1");
        }


        if (jobHandle.IsCompleted)
        {
            Debug.LogError("jobHandle Complete 2");
        }
        // 调度（Schedule）一个Job是比较简单的，只需要调用Schedule()方法就可以了。这里比较有意思的是Complete()方法，
        // 在我们需要读取执行结果之前需要调用Complete()方法。但是Complete()不一定在Schedule()之后立即调用，
        // 也不一定在当前帧必须调用，也就是说一个Job本身不受Update()限制可以跨帧运行。当一个Job需要跨帧运行的时候，
        // 我们需要使用IsCompleted属性来判断Job是否执行完毕

        // 在此处调用 Complete()方法，会在主线程等待 Job 执行结束，才会继续执行下面的代码
        jobHandle.Complete();
        // 获取 Job 执行的结果,可以得到正确结果

        // 强调：即使IsCompleted返回true，在获取结果钱也必须要调用Complete()方法
        Debug.LogError("CalculateAdd resultArr:" + add.resultArr[0]);
        // 结构体引用了 resultArr，通过 resultArr 也是可以获取到的
        Debug.LogError("resultArr :" + resultArr[0]);
        // add.result 获取在一些版本的 Unity 下会报错
        // 在 Unity2023.3.26版本测试，没有报错，返回结果是 0,也就是 result 的赋值并不会被保存 
        Debug.LogError("result:" + add.result);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        // NativeContainer是需要显式管理内存的
        if (resultArr.IsCreated) // 如果创建了需要手动销毁
        {
            resultArr.Dispose();
        }

    }
}
