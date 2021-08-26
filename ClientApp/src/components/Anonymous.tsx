import * as React from "react";
import { useHistory } from "react-router";
import { useSelector } from "~/store";

export interface AnonymousProps {
  fallback: string;
  children: React.ReactNode;
}

export function Anonymous(props: AnonymousProps) {
  const user = useSelector(state => state.user);
  const history = useHistory();

  React.useEffect(() => {
    if (user) {
      history.replace(props.fallback);
    }
  }, [user]);

  return (
    <React.Fragment>
      {props.children}
    </React.Fragment>
  );
}