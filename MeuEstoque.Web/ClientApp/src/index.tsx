import * as React from "react";
import * as ReactDOM from "react-dom";
import { Switch, Route, Redirect } from "react-router";
import { BrowserRouter as Router } from "react-router-dom";
import { Provider } from "react-redux";

import "~/style/style.scss";
import configureStore, { updateUserAsync } from "~/store";
import { Dashboard, SignUp, Login } from "~/pages";
import { Anonymous } from "~/components/Anonymous";
import { Authorized } from "~/components/Authorized";

const store = configureStore();

function App() {
  return (
    <Provider store={store}>
      <Router>
        <Switch>
          <Redirect exact from="/" to="/dashboard" />
          <Redirect exact from="/dashboard" to="/dashboard/products" />
          <Route exact path="/signup">
            <Anonymous fallback="/">
              <SignUp />
            </Anonymous>
          </Route>
          <Route exact path="/login">
            <Anonymous fallback="/">
              <Login />
            </Anonymous>
          </Route>
          <Route path="/dashboard">
            <Authorized fallback="/login">
              <Dashboard />
            </Authorized>
          </Route>
        </Switch>
      </Router>
    </Provider>
  );
}

ReactDOM.render(<App />, document.querySelector("#root"));

store.dispatch(updateUserAsync() as any);