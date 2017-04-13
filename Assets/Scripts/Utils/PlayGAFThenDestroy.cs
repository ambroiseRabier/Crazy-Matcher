using UnityEngine;
using GAFInternal.Core;
using System.Collections;

[RequireComponent(typeof(IGAFMovieClip))]
public class PlayGAFThenDestroy : MonoBehaviour {
	private IEnumerator Start () {
        IGAFMovieClip movieClip = GetComponent<IGAFMovieClip>();
        movieClip.setAnimationWrapMode(GAFWrapMode.Once);

        while (movieClip.isPlaying())
            yield return null;

        if (gameObject)
            Destroy(gameObject);
    }
}
