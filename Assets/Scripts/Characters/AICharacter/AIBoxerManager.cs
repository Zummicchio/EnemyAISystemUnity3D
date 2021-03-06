﻿using System.Collections.Generic;
using UnityEngine;

namespace AIManager_namespace
{
    public abstract class AIBoxerManager : AIManager, IBoxerManager
    {
        [Header("AI Boxer Variables")]
        public AICombatField m_AIBoxerField;

        public GiveHitCollider[] m_GiveHitColliders;

        public int m_GiveHitID;
        public int m_GiveBlockID;

        protected override void Initialized()
        {
            m_GiveHitColliders = GetComponentsInChildren<GiveHitCollider>();

            float sqrRange = m_AIBoxerField.AttackRange * m_AIBoxerField.AttackRange;

            m_unwareAIState = new AIUnwareState<AIManager>(this, sqrRange);
            m_patrolAIState = new AIPatrolState<AIManager>(this, sqrRange);
            m_chaseAIState = new AIChaseState<AIManager>(this, sqrRange);
            m_searchAIState = new AISearchState<AIManager>(this, sqrRange);
            m_coverAIState = new AICoverState<AIManager>(this);

            float sqrKickDist = m_AIBoxerField.KickRange * m_AIBoxerField.KickRange;
            float sqrPunchDist = m_AIBoxerField.PunchRange * m_AIBoxerField.PunchRange;

            m_boxingAIState = new AIBoxingState(this, m_charRadius, sqrRange, sqrKickDist, sqrPunchDist);

            DisableGiveHitCollider(HitColliderType.LeftAnkle);
            base.Initialized();
        }

        public override void GiveDamage<T>(T manager, Vector3 hitPosition)
        {
            manager.TakeDamage(10, DamageType.DamageByHand);
        }

        public void Punch(float sqrDistFromOpp)
        {
            int rand = Random.Range(0, 3);

            m_animator.SetInteger("PunchID", rand);
            m_animator.SetTrigger("Punch");
        }

        public void Kick(float sqrDistFromOpp)
        {
            int rand = Random.Range(0, 4);
            m_animator.SetInteger("KickID", rand);
            m_animator.SetTrigger("Kick");
        }

        public void EnableGiveHitCollider(HitColliderType name)
        {
            foreach (GiveHitCollider col in m_GiveHitColliders)
            {
                if (col.m_HitColliderType == name)
                {
                    col.enabled = true;
                    break;
                }
            }
        }

        public void DisableGiveHitCollider(HitColliderType name)
        {
            foreach (GiveHitCollider col in m_GiveHitColliders)
            {
                col.enabled = false;
            }
        }

        public void TakeAwarness(int blockID)
        {
        }

        public void TakeDamageByBoxing(int damage, int hitID)
        {
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, m_AIBoxerField.AttackRange);
            Gizmos.DrawWireSphere(transform.position, m_AIBoxerField.KickRange);
            Gizmos.DrawWireSphere(transform.position, m_AIBoxerField.PunchRange);
        }
    }
}
