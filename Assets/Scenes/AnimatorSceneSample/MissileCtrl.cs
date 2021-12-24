using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MissileCtrl : MonoBehaviour
{
    private Animator animator;

    private Transform trs;

    void Start()
    {
        animator = GetComponent<Animator>();
        trs = GetComponent<Transform>();
    }

    public Transform GetBodyTransformInfo()
    {
        return trs.GetChild(0);
    }

    public Transform GetWingTransformInfo()
    {
        return trs.GetChild(1);
    }
}

partial class MissileUpdateSystem : SystemBase
{
    private MissileCtrl missileCtl;

    public MissileUpdateSystem()
    {
        var m_Root = (GameObject)GameObject.FindWithTag("missile");
        missileCtl = m_Root.GetComponentInChildren<MissileCtrl>();
    }

    protected override void OnUpdate()
    {
        if (missileCtl == null)
        {
            return;
        }

        Debug.Log("animation transform");
        Debug.Log("position : " + missileCtl.GetBodyTransformInfo().position.ToString());

        float offset = 2.0F;

        var bodyPosition = missileCtl.GetBodyTransformInfo().position;
        Entities.WithAll<MissileBodyTag>().ForEach((ref Translation translation) =>
        {
            bodyPosition.Set(bodyPosition.x + offset, bodyPosition.y, bodyPosition.z + offset);
            translation.Value = bodyPosition;
        }).ScheduleParallel();

        var bodyRotation = missileCtl.GetBodyTransformInfo().rotation;
        Entities.WithAll<MissileBodyTag>().ForEach((ref Rotation rotation) =>
        {
            rotation.Value = bodyRotation;
        }).ScheduleParallel();

        var wingPosition = missileCtl.GetWingTransformInfo().position;
        Entities.WithAll<MissileWingTag>().ForEach((ref Translation translation) =>
        {
            wingPosition.Set(wingPosition.x + offset, wingPosition.y, wingPosition.z + offset);
            translation.Value = wingPosition;
        }).ScheduleParallel();

        var wingRotation = missileCtl.GetWingTransformInfo().rotation;
        Entities.WithAll<MissileWingTag>().ForEach((ref Rotation rotation) =>
        {
            rotation.Value = wingRotation;
        }).ScheduleParallel();
    }
}
