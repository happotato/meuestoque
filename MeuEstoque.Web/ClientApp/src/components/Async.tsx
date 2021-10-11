import * as React from "react";

export interface AsyncComplete<T> {
  state: "complete";
  value: T;
}

export interface AsyncError<T> {
  state: "error";
  value: T;
}

export interface AsyncLoading {
  state: "loading";
}

export type AsyncResult<T, E> = AsyncComplete<T> | AsyncError<E> | AsyncLoading;

export interface AsyncProps<T, E> {
  func: (signal: AbortSignal) => Promise<T>;
  children: (result: AsyncResult<T, E>) => JSX.Element;
}

export function Async<T, E = unknown>(props: AsyncProps<T, E>) {
  const [state, setState] = React.useState<AsyncResult<T, E>>({
    state: "loading",
  });

  React.useEffect(() => {
    const abortController = new AbortController();

    async function call() {
      try {
        const res = await props.func(abortController.signal);
        setState({
          state: "complete",
          value: res,
        });
      } catch (e) {
        if (e instanceof DOMException && e.code == DOMException.ABORT_ERR) {
          return;
        }

        setState({
          state: "error",
          value: e,
        });
      }
    }

    call();

    return () => {
      abortController.abort();
    };
  }, []);

  return props.children(state);
}
