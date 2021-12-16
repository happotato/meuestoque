import * as React from "react";

export function useMediaQuery(query: string) {
  const [state, setState] = React.useState(window.matchMedia(query).matches);

  React.useEffect(() => {
    const mediaQuery = window.matchMedia(query);

    const callback = (mediaQuery: MediaQueryList)  => {
      setState(mediaQuery.matches);
    };

    mediaQuery.addEventListener("change", callback as any);

    return () => {
      mediaQuery.removeEventListener("change", callback as any);
    };
  }, []);

  return state;
}
