namespace UniGame.DoTween.Runtime.Extensions
{
    using Cysharp.Threading.Tasks;
    using DG.Tweening;

    public static class DOTweenAsyncExtensions
    {
        public static async UniTask<Tween> WaitForCompletionTweenAsync(this Tween t)
        {
            while (t.active && !t.IsComplete())
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            return t;
        }

        public static async UniTask<Tween> WaitForRewindTweenAsync(this Tween t)
        {
            while (t.active && (!t.playedOnce || t.position * (t.CompletedLoops() + 1) > 0.0)) {
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            }
            return t;
        }

        public static async UniTask<Tween> WaitForKillTweenAsync(this Tween t)
        {
            while (t.active) {
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            }
            return t;
        }
        public static async UniTask<Tween> WaitForElapsedLoopTweenAsync(this Tween t, int elapsedLoops)
        {
            while (t.active && t.CompletedLoops() < elapsedLoops) {
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            }
            return t;
        }

        public static async UniTask<Tween> WaitForPositionTweenAsync(this Tween t, float position)
        {
            while (t.active && t.position * (t.CompletedLoops() + 1) < position) {
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            }
            return t;
        }
        
        public static async UniTask<Tween> WaitForStartTweenAsync(this Tween t)
        {
            while (t.active && !t.playedOnce) {
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            }
            return t;
        }
    }
}