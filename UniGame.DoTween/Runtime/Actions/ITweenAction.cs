namespace UniGame.DoTween.Runtime.Actions
{
    using DG.Tweening;
    using UnityEngine;

    public interface ITweenAction
    {
        public Tween Execute(GameObject target);
    }
}