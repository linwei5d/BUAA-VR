using System.Collections;
using System.Collections.Generic;
using Tarodev;
using Unity.VisualScripting;
using UnityEngine;
public class Missile_Self : MonoBehaviour
{
    //最大转弯速度
    private float MaximumRotationSpeed = 200.0f;
    //加速度
    private float AcceleratedVeocity = 4.0f;
    //最高速度
    private float MaximumVelocity = 90.0f;
    //生命周期
    private float MaximumLifeTime = 20.0f;
    //上升期时间
    private float AccelerationPeriod = 0.5f;
    //目标
	private Transform Target;
    //当前速度
    private float CurrentVelocity = 6.0f;
    private Vector3 offset;
    //生命期
    private float lifeTime = 0.0f;
    public void SetOffset(Vector3 forward){
        offset = Quaternion.Euler(-10.0f, Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * forward;
    }
    private void Start(){
        offset = Quaternion.Euler(-10.0f, Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * transform.forward;
        // offset = new Vector3(0, 0, 2f);
    }
    private float Detect_Time;
    private void Update()
    {
        float deltaTime = Time.deltaTime;
        lifeTime += deltaTime;
        Detect_Time += deltaTime;
        //如果超出生命周期,则3s后销毁
        if(lifeTime > MaximumLifeTime)
        {
            Destroy(gameObject, 3.0f);
            return;
        }
        else if(Detect_Time > 3.0f && Target == null){
            SetTarget(Generate.Targets[Random.Range(0, Generate.Targets.Count+1)]);
        }
        //计算朝向目标的方向偏移量,如果处于上升期,则忽略目标

        //如果角度很小,就直接对准目标
        if(lifeTime < AccelerationPeriod)
            transform.forward = offset;
        else if(Target != null)
        {
            offset = offset/1.17f + (Target.position - transform.position).normalized * 1.17f;
            float angle = Vector3.Angle(transform.forward, offset);
            float needTime = angle / (MaximumRotationSpeed * (CurrentVelocity / MaximumVelocity));
            if (needTime < 0.5f) {
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
        //如果碰撞,则销毁
        Destroy(gameObject, 0.02f);
        if(other != null && other.transform.TryGetComponent<Target>(out var ex))
            ex.Damaged(1);
    }
}
