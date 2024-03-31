using System.Collections;
using System.Collections.Generic;
using Tarodev;
using Unity.VisualScripting;
using UnityEngine;
public class Missile_Target : MonoBehaviour
{
    //最大转弯速度
    private float MaximumRotationSpeed = 120.0f;
    //加速度
    private float AcceleratedVeocity = 2.0f;
    //最高速度
    private float MaximumVelocity = 20.0f;
    //生命周期
    private float MaximumLifeTime = 10.0f;
    //上升期时间
    private float AccelerationPeriod = 0.5f;
    //目标
	private Transform Target;
    //当前速度
    private float CurrentVelocity = 4.0f;
    private Vector3 offset = new Vector3(-0.1f, -0.1f, -1f);
    //生命期
    private float lifeTime = 0.0f;
    private void Update()
    {
        float deltaTime = Time.deltaTime;
        lifeTime += deltaTime;
        //如果超出生命周期,则3s后销毁
        if(lifeTime > MaximumLifeTime )
        {
            Destroy(gameObject, 3.0f);
            return;
        }
        //计算朝向目标的方向偏移量,如果处于上升期,则忽略目标

        //如果角度很小,就直接对准目标
        if(lifeTime < AccelerationPeriod)
            transform.forward = offset;
        else if(Target != null)
        {
            offset = (Target.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, offset);
            float needTime = angle / (MaximumRotationSpeed * (CurrentVelocity / MaximumVelocity));
            if (needTime < 0.2f) {
                transform.forward = offset;
            }
            else if(Vector3.Distance(transform.position, Target.position) < CurrentVelocity*8.0f)//当前帧间隔时间除以需要的时间,获取本次球形插值,并旋转游戏对象方向至面向目标
                transform.forward = Vector3.Slerp(transform.forward, offset, deltaTime / needTime).normalized;
        }

        //如果当前速度小于最高速度,则进行加速
        if (CurrentVelocity < MaximumVelocity) CurrentVelocity += deltaTime * AcceleratedVeocity;

	    //朝自己的前方位移
        transform.position += transform.forward * CurrentVelocity * deltaTime;
    }
    public void SetTarget(Transform target){
        Target = target;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(Time.time > 1.0f){
            Destroy(gameObject);
            Generate.RemoveDestroyedObjects();
        }
    }
}
