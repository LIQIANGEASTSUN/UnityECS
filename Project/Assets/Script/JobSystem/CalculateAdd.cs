using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Unity.Burst;

//Job中的变量我们仅可以使用blittable types或者Unity为我们提供的NativeContainer容器，
//比如引擎内置的NativeArray或者com.unity.collections package中提供的容器类。
//
//让我们来总结一下声明一个Job的要点：
//1.创建一个实现了IJob接口的struct。
//2.在struct中声明blittable types或者NativeContainer的变量。
//3.在Execute()方法中实现Job的逻辑。
//
// 下面例子中 结构体中 CalculateAdd 变量 result 的值是无法存储的，执行之后 获取不到改变的结果
// 必须使用 NativeArray 声明一个变量，仅仅只有一个参数需要赋值，也是要创建只包含一个数据的 NativeArray

[BurstCompile]
public struct CalculateAdd : IJob
{
    public int numberA;
    public int numberB;

    public NativeArray<int> resultArr;

    public int result;

    public void Execute()
    {
        Debug.LogError("CalculateAdd Execute");
        for (int i = 0; i < resultArr.Length; i++)
        {
            resultArr[i] = numberA + numberB;
        }

        result = numberA + numberB;
    }
}