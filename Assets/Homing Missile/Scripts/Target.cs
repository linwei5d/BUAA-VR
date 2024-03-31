using System;
using Unity.Mathematics;
using UnityEngine;

namespace Tarodev {
    public class Target : MonoBehaviour, IExplode {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _size = 200.0f;
        [SerializeField] private float _speed = 300.0f;
        private int HP;
        public Rigidbody Rb => _rb;
        public AudioSource audio;
        public AudioClip Default, Dead;

        private GameObject missile; // 拖拽你创建的预制体到这个变量中
        public Transform target;
    // Update 函数在每一帧都会调用
    // 创建物体的函数
    public void Create()
    {
        GameObject obj = Instantiate(missile, transform.position + new Vector3(10f,-18f,0f), transform.rotation);
        obj.GetComponent<Missile_Target>().SetTarget(target);
        Generate.Targets.Add(obj.transform);
    }
        private void PlayAudio(){
            audio.Play();
        }
        private void Start() {
            missile = Resources.Load<GameObject>("Prefabs/Missile 1");
            HP = 100;
            transform.position = new Vector3(0, 20.0f, 12000.0f);
            Generate.Targets.Add(transform);
            audio.clip = Default;
            Invoke("PlayAudio", 35.0f);
        }
        private float Missile_Load = 0f;
        void Update() {
            if(transform.position.z > 91.0f){
                transform.position = new Vector3(0, 20.0f, Mathf.Max(90.0f, 12000.0f - 300.0f * Time.fixedTime));
            }
            else if(HP > 0){
                _rb.velocity = new Vector3(-Mathf.Cos(Time.time), Mathf.Sin(Time.time * 1.8f + math.PI/4))* 20f;
                Missile_Load += Time.deltaTime;
                if(Missile_Load > 5f){
                    Missile_Load = 0f;
                    Create();
                }
            }
        }
        public void Damaged(int damage){
            if(HP > 0 && (HP -= damage) <= 0){
                _rb.velocity = new Vector3(0, -20.0f);
                transform.eulerAngles = new Vector3(-50.0f, 0);
                audio.clip = Dead;
                audio.Play();
                Destroy(gameObject, 6.0f);
                Generate.RemoveDestroyedObjects();
            }

        }
        public void Explode(){

        }
    }
}