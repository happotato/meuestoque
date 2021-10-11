import {
  useSelector as useReduxSelector,
  useDispatch as useReduxDispatch,
  TypedUseSelectorHook,
} from "react-redux";
import { createStore, Action, applyMiddleware } from "redux";
import thunk, { ThunkAction } from "redux-thunk";

import { User, SignUp, getCurrentUser, logIn, logOut, signUp } from "./api";

export interface UserLoadBeginAction extends Action<"USER_LOAD_BEGIN_ACTION"> {}

export interface UserLoadErrorAction extends Action<"USER_ERROR_ACTION"> {
  msg: string;
}

export interface LoginCompletedAction extends Action<"LOGIN_COMPLETED_ACTION"> {
  user: User;
}

export interface LogoutCompletedAction
  extends Action<"LOGOUT_COMPLETED_ACTION"> {}

export type ApplicationAction =
  | UserLoadBeginAction
  | UserLoadErrorAction
  | LoginCompletedAction
  | LogoutCompletedAction;

export type ApplicationThunkAction = ThunkAction<
  void,
  ApplicationState,
  unknown,
  ApplicationAction
>;

export interface ApplicationState {
  user?: User;
  userErrorMsg?: string;
  isLoadingUser: boolean;
}

export const useSelector: TypedUseSelectorHook<ApplicationState> =
  useReduxSelector;
export const useDispatch = () => useReduxDispatch();

export function signUpActionAsync(signin: SignUp): ApplicationThunkAction {
  return async (dispatch) => {
    dispatch({
      type: "USER_LOAD_BEGIN_ACTION",
    });

    try {
      const user = await signUp(signin);

      dispatch({
        type: "LOGIN_COMPLETED_ACTION",
        user: user,
      });
    } catch {
      dispatch({
        type: "USER_ERROR_ACTION",
        msg: "Failed to sign up, try again.",
      });
    }
  };
}

export function logInActionAsync(
  email: string,
  password: string
): ApplicationThunkAction {
  return async (dispatch) => {
    dispatch({
      type: "USER_LOAD_BEGIN_ACTION",
    });

    try {
      const user = await logIn(email, password);

      dispatch({
        type: "LOGIN_COMPLETED_ACTION",
        user: user,
      });
    } catch {
      dispatch({
        type: "USER_ERROR_ACTION",
        msg: "Failed to login, try again.",
      });
    }
  };
}

export function logoutActionAsync(): ApplicationThunkAction {
  return async (dispatch) => {
    await logOut();

    dispatch({
      type: "LOGOUT_COMPLETED_ACTION",
    });
  };
}

export function updateUserAsync(): ApplicationThunkAction {
  return async (dispatch) => {
    dispatch({
      type: "USER_LOAD_BEGIN_ACTION",
    });

    try {
      const user = await getCurrentUser();
      dispatch({
        type: "LOGIN_COMPLETED_ACTION",
        user: user,
      });
    } catch {
      dispatch({
        type: "LOGOUT_COMPLETED_ACTION",
      });
    }
  };
}

const initialApplicationState: ApplicationState = {
  user: undefined,
  userErrorMsg: undefined,
  isLoadingUser: false,
};

function reducer(
  state: ApplicationState = initialApplicationState,
  action: ApplicationAction
): ApplicationState {
  switch (action.type) {
    case "USER_ERROR_ACTION": {
      return {
        ...state,
        isLoadingUser: false,
        user: undefined,
        userErrorMsg: action.msg,
      };
    }

    case "USER_LOAD_BEGIN_ACTION": {
      return {
        ...state,
        userErrorMsg: undefined,
        isLoadingUser: true,
      };
    }

    case "LOGIN_COMPLETED_ACTION": {
      return {
        ...state,
        user: action.user,
        userErrorMsg: undefined,
        isLoadingUser: false,
      };
    }

    case "LOGOUT_COMPLETED_ACTION": {
      return {
        ...state,
        user: undefined,
        userErrorMsg: undefined,
        isLoadingUser: false,
      };
    }

    default:
      return state;
  }
}

export default function configureStore() {
  return createStore(reducer, applyMiddleware(thunk));
}
