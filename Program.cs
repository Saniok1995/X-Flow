using UnityEngine;

public class Program
{
        private double _previousTargetSetTime;
        private bool _isTargetSet;
        private TargetCandidate _lockedCandidateTarget;
        private TargetCandidate _lockedTarget;
        private TargetCandidate _target;
        private TargetCandidate _previousTarget;
        private TargetCandidate _activeTarget;
        private TargetCandidate _targetInRangeContainer;
        
        private bool CanNotBeTarget(TargetCandidate candidate)
        {
            return candidate && !candidate.CanBeTarget;
        }
        
        private bool CanBeTarget(TargetCandidate candidate)
        {
            return candidate && candidate.CanBeTarget;
        }

        private bool IsTimeToChange()
        {
            const double targetChangeTime = 1;
            return Time.time - _previousTargetSetTime < targetChangeTime;
        }

        private void TrySetUpPreviousSetTime()
        {
            if (_previousTarget != _target)
            {
                _previousTargetSetTime = Time.time;
            }
        }

        private void SetUpTarget()
        {
            if (CanBeTarget(_lockedTarget))
            {
                _target = _lockedTarget;
                _isTargetSet = true;
                return;
            }
                    
            if (CanBeTarget(_activeTarget))
            {
                _target = _activeTarget;
                _isTargetSet = true;
                return;
            }
                    
            _target = _targetInRangeContainer.GetTarget();
            if (_target)
            {
                _isTargetSet = true;
            }
        }

        private void HandleFinishedTargetState()
        {
            if (_isTargetSet)
            {
                TrySetUpPreviousSetTime();
            }
            else
            {
                _target = null;
            }
        }

        public void CleanupTest(Frame frame)
        {
            if (CanNotBeTarget(_lockedCandidateTarget))
            {
                _lockedCandidateTarget = null;
            }

            if (CanNotBeTarget(_lockedTarget))
            {
                _lockedTarget = null;
            }
            
            _isTargetSet = false;
            
            try
            {
				// Sets _activeTarget field
                TrySetActiveTargetFromQuantum(frame);

                // If target exists and can be targeted, it should stay within Target Change Time since last target change
                if (CanBeTarget(_target) && IsTimeToChange())
                {
                    _isTargetSet = true;
                }
                
                _previousTarget = _target;

                if (!_isTargetSet)
                {
                    SetUpTarget();
                }
            }
            finally
            {
                HandleFinishedTargetState();
                TargetableEntity.Selected = _target;
            }
        }

        private void TrySetActiveTargetFromQuantum(Frame frame)
        {
            
        }
}

public struct Frame
{
    
}

public static class Extension
{
    public static object GetTarget(this object target)
    {
        return default;
    }
}

public static class TargetableEntity
{
    public static object Selected;
}

public class TargetCandidate
{
    public bool CanBeTarget { get; } = true;
    
    public static implicit operator bool(TargetCandidate candidate)
    {
        return candidate != null;
    }

    public TargetCandidate GetTarget()
    {
        return null;
    }
}